// DShowPlayer.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <cmath>


#include "dshowplayer.h"

// An application can advertise the existence of its filter graph
// by registering the graph with a global Running Object Table (ROT).
// The GraphEdit application can detect and remotely view the running
// filter graph, allowing you to 'spy' on the graph with GraphEdit.
//
// To enable registration in this sample, define REGISTER_FILTERGRAPH.
//
//#define REGISTER_FILTERGRAPH

//
// Global data
//
HWND      ghWnd=0;
HMENU     ghMenu=0;
HINSTANCE ghInst=0;
TCHAR     g_szFileName[MAX_PATH]={0};
BOOL      g_bAudioOnly=FALSE, g_bFullscreen=FALSE;
LONG      g_lVolume=VOLUME_FULL;
DWORD     g_dwGraphRegister=0;
PLAYSTATE g_psCurrent=Stopped;
double    g_PlaybackRate=1.0;

// DirectShow interfaces
IGraphBuilder *pGB   = NULL;
IMediaControl *pMC   = NULL;
IMediaEventEx *pME   = NULL;
IVideoWindow  *pVW   = NULL;
IBasicAudio   *pBA   = NULL;
IBasicVideo   *pBV   = NULL;
IMediaSeeking *pMS   = NULL;
IMediaPosition *pMP  = NULL;
IVideoFrameStep *pFS = NULL;

const int AUDIO=1, VIDEO=2; // Used for enabling playback menu items


HRESULT PlayMovieInWindow(LPCWSTR file)
{
    USES_CONVERSION;
    HRESULT hr;
	if(g_psCurrent != Init)
	{
		CloseClip();
	}


    // Clear open dialog remnants before calling RenderFile()
    UpdateWindow(ghWnd);

    // Get the interface for DirectShow's GraphBuilder
    JIF(CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC_SERVER,
                         IID_IGraphBuilder, (void **)&pGB));

    if(SUCCEEDED(hr))
    {
        // Have the graph builder construct its the appropriate graph automatically
        JIF(pGB->RenderFile(file, NULL));

        // QueryInterface for DirectShow interfaces
        JIF(pGB->QueryInterface(IID_IMediaControl, (void **)&pMC));
        JIF(pGB->QueryInterface(IID_IMediaEventEx, (void **)&pME));
        JIF(pGB->QueryInterface(IID_IMediaSeeking, (void **)&pMS));
        JIF(pGB->QueryInterface(IID_IMediaPosition, (void **)&pMP));

        // Query for video interfaces, which may not be relevant for audio files
        JIF(pGB->QueryInterface(IID_IVideoWindow, (void **)&pVW));
        JIF(pGB->QueryInterface(IID_IBasicVideo, (void **)&pBV));

        // Query for audio interfaces, which may not be relevant for video-only files
        JIF(pGB->QueryInterface(IID_IBasicAudio, (void **)&pBA));

        // Is this an audio-only file (no video component)?
        CheckVisibility();

        // Have the graph signal event via window callbacks for performance
        JIF(pME->SetNotifyWindow((OAHWND)ghWnd, WM_GRAPHNOTIFY, 0));

        if (!g_bAudioOnly)
        {
            // Setup the video window
            JIF(pVW->put_Owner((OAHWND)ghWnd));
            JIF(pVW->put_WindowStyle(WS_CHILD | WS_CLIPSIBLINGS | WS_CLIPCHILDREN));

            JIF(InitVideoWindow(1, 1));
            GetFrameStepInterface();
        }

        // Complete window initialization
        UpdateWindow(ghWnd);
        g_bFullscreen = FALSE;
        g_PlaybackRate = 1.0;

#ifdef REGISTER_FILTERGRAPH
        hr = AddGraphToRot(pGB, &g_dwGraphRegister);
        if (FAILED(hr))
        {
            Msg(TEXT("Failed to register filter graph with ROT!  hr=0x%x"), hr);
            g_dwGraphRegister = 0;
        }
#endif

        // Run the graph to play the media file
        JIF(pMC->Run());

        g_psCurrent=Running;
    }

    return hr;
}

bool _canStep()
{
	return pFS != NULL;
}



HRESULT InitVideoWindow(int nMultiplier, int nDivider)
{
    /*LONG lHeight, lWidth;*/
    HRESULT hr = S_OK;
    RECT rect;


    GetClientRect(ghWnd, &rect);
    JIF(pVW->SetWindowPosition(rect.left, rect.top, rect.right, rect.bottom));

    return hr;
}


void MoveVideoWindow(void)
{
    HRESULT hr;

    // Track the movement of the container window and resize as needed
    if(pVW)
    {
        RECT client;

        GetClientRect(ghWnd, &client);
        hr = pVW->SetWindowPosition(client.left, client.top, client.right, client.bottom);
    }
}

void CheckVisibility(void)
{
    long lVisible;
    HRESULT hr;

    if ((!pVW) || (!pBV))
    {
        // Audio-only files have no video interfaces.  This might also
        // be a file whose video component uses an unknown video codec.
        g_bAudioOnly = TRUE;
        return;
    }
    else
    {
        // Clear the global flag
        g_bAudioOnly = FALSE;
    }

    hr = pVW->get_Visible(&lVisible);
    if (FAILED(hr))
    {
        // If this is an audio-only clip, get_Visible() won't work.
        //
        // Also, if this video is encoded with an unsupported codec,
        // we won't see any video, although the audio will work if it is
        // of a supported format.
        //
        if (hr == E_NOINTERFACE)
        {
            g_bAudioOnly = TRUE;
        }
        else
        {
            Msg(TEXT("Failed(%08lx) in pVW->get_Visible()!\r\n"), hr);
        }
    }
}


void PauseClip(void)
{
    if (!pMC)
        return;

    // Toggle play/pause behavior
    if((g_psCurrent == Paused) || (g_psCurrent == Stopped))
    {
        if (SUCCEEDED(pMC->Run()))
            g_psCurrent = Running;
    }
    else
    {
        if (SUCCEEDED(pMC->Pause()))
            g_psCurrent = Paused;
    }

}


void StopClip(void)
{
    HRESULT hr;

    if ((!pMC) || (!pMS))
        return;

    // Stop and reset postion to beginning
    if((g_psCurrent == Paused) || (g_psCurrent == Running))
    {
        LONGLONG pos = 0;
        hr = pMC->Stop();
        g_psCurrent = Stopped;

        // Seek to the beginning
        hr = pMS->SetPositions(&pos, AM_SEEKING_AbsolutePositioning ,
            NULL, AM_SEEKING_NoPositioning);

        // Display the first frame to indicate the reset condition
        hr = pMC->Pause();
    }

}

void SeekClip(double pos)
{
	HRESULT hr;

    if (!pMP) return;
	REFTIME time;
	hr = pMP->get_Duration(&time);
	if (pos >= time) return;
    if((g_psCurrent == Paused) || (g_psCurrent == Running))
    {
		time = pos;
		hr = pMP->put_CurrentPosition(time);
    }
}


void CloseClip()
{
    HRESULT hr;

    // Stop media playback
    if(pMC)
        hr = pMC->Stop();

    // Clear global flags
    g_psCurrent = Stopped;
    g_bAudioOnly = TRUE;
    g_bFullscreen = FALSE;

    // Free DirectShow interfaces
    CloseInterfaces();

    // Clear file name to allow selection of new file with open dialog
    g_szFileName[0] = L'\0';

    // No current media state
    g_psCurrent = Init;

    // Reset the player window
    RECT rect;
    GetClientRect(ghWnd, &rect);
    InvalidateRect(ghWnd, &rect, TRUE);

}


void CloseInterfaces(void)
{
    HRESULT hr;

    // Relinquish ownership (IMPORTANT!) after hiding video window
    if(pVW)
    {
        hr = pVW->put_Visible(OAFALSE);
        hr = pVW->put_Owner(NULL);
    }

    // Disable event callbacks
    if (pME)
        hr = pME->SetNotifyWindow((OAHWND)NULL, 0, 0);

#ifdef REGISTER_FILTERGRAPH
    if (g_dwGraphRegister)
    {
        RemoveGraphFromRot(g_dwGraphRegister);
        g_dwGraphRegister = 0;
    }
#endif

    // Release and zero DirectShow interfaces
    SAFE_RELEASE(pME);
    SAFE_RELEASE(pMS);
    SAFE_RELEASE(pMP);
    SAFE_RELEASE(pMC);
    SAFE_RELEASE(pBA);
    SAFE_RELEASE(pBV);
    SAFE_RELEASE(pVW);
    SAFE_RELEASE(pFS);
    SAFE_RELEASE(pGB);
}


#ifdef REGISTER_FILTERGRAPH

HRESULT AddGraphToRot(IUnknown *pUnkGraph, DWORD *pdwRegister)
{
    IMoniker * pMoniker;
    IRunningObjectTable *pROT;
    if (FAILED(GetRunningObjectTable(0, &pROT)))
    {
        return E_FAIL;
    }

    WCHAR wsz[128];
    HRESULT hr = StringCchPrintfW(wsz, NUMELMS(wsz), L"FilterGraph %08x pid %08x", (DWORD_PTR)pUnkGraph,
              GetCurrentProcessId());

    hr = CreateItemMoniker(L"!", wsz, &pMoniker);
    if (SUCCEEDED(hr))
    {
        // Use the ROTFLAGS_REGISTRATIONKEEPSALIVE to ensure a strong reference
        // to the object.  Using this flag will cause the object to remain
        // registered until it is explicitly revoked with the Revoke() method.
        //
        // Not using this flag means that if GraphEdit remotely connects
        // to this graph and then GraphEdit exits, this object registration
        // will be deleted, causing future attempts by GraphEdit to fail until
        // this application is restarted or until the graph is registered again.
        hr = pROT->Register(ROTFLAGS_REGISTRATIONKEEPSALIVE, pUnkGraph,
                            pMoniker, pdwRegister);
        pMoniker->Release();
    }

    pROT->Release();
    return hr;
}

void RemoveGraphFromRot(DWORD pdwRegister)
{
    IRunningObjectTable *pROT;

    if (SUCCEEDED(GetRunningObjectTable(0, &pROT)))
    {
        pROT->Revoke(pdwRegister);
        pROT->Release();
    }
}

#endif


void Msg(TCHAR *szFormat, ...)
{
    TCHAR szBuffer[1024];  // Large buffer for long filenames or URLs
    const size_t NUMCHARS = sizeof(szBuffer) / sizeof(szBuffer[0]);
    const int LASTCHAR = NUMCHARS - 1;

    // Format the input string
    va_list pArgs;
    va_start(pArgs, szFormat);

    // Use a bounded buffer size to prevent buffer overruns.  Limit count to
    // character size minus one to allow for a NULL terminating character.
    (void)StringCchVPrintf(szBuffer, NUMCHARS - 1, szFormat, pArgs);
    va_end(pArgs);

    // Ensure that the formatted string is NULL-terminated
    szBuffer[LASTCHAR] = TEXT('\0');

    // Display a message box with the formatted string
    MessageBox(NULL, szBuffer, TEXT("PlayWnd Sample"), MB_OK);
}


HRESULT ToggleMute(void)
{
    HRESULT hr=S_OK;

    if ((!pGB) || (!pBA))
        return S_OK;

    // Read current volume
    hr = pBA->get_Volume(&g_lVolume);
    if (hr == E_NOTIMPL)
    {
        // Fail quietly if this is a video-only media file
        return S_OK;
    }
    else if (FAILED(hr))
    {
        Msg(TEXT("Failed to read audio volume!  hr=0x%x\r\n"), hr);
        return hr;
    }

    // Switch volume levels
    if (g_lVolume == VOLUME_FULL)
        g_lVolume = VOLUME_SILENCE;
    else
        g_lVolume = VOLUME_FULL;

    // Set new volume
    JIF(pBA->put_Volume(g_lVolume));


    return hr;
}

HRESULT set_volume(double volume)
{
	HRESULT hr=S_OK;
	if ((!pGB) || (!pBA))        return S_OK;
	long v = -(long)(pow(10, (1 - volume))) * 1000 + 1000;
	if (v >= 0) v = 0;
	if (v <= -10000) v = -10000;
	g_lVolume = v;
	JIF(pBA->put_Volume(g_lVolume));
	return hr;
}

HRESULT ToggleFullScreen(void)
{
    HRESULT hr=S_OK;
    LONG lMode;
    static HWND hDrain=0;

    // Don't bother with full-screen for audio-only files
    if ((g_bAudioOnly) || (!pVW))
        return S_OK;

    // Read current state
    JIF(pVW->get_FullScreenMode(&lMode));

    if (lMode == OAFALSE)
    {
        // Save current message drain
        LIF(pVW->get_MessageDrain((OAHWND *) &hDrain));

        // Set message drain to application main window
        LIF(pVW->put_MessageDrain((OAHWND) ghWnd));

        // Switch to full-screen mode
        lMode = OATRUE;
        JIF(pVW->put_FullScreenMode(lMode));
        g_bFullscreen = TRUE;
    }
    else
    {
        // Switch back to windowed mode
        lMode = OAFALSE;
        JIF(pVW->put_FullScreenMode(lMode));

        // Undo change of message drain
        LIF(pVW->put_MessageDrain((OAHWND) hDrain));

        // Reset video window
        LIF(pVW->SetWindowForeground(-1));

        // Reclaim keyboard focus for player application
        UpdateWindow(ghWnd);
        SetForegroundWindow(ghWnd);
        SetFocus(ghWnd);
        g_bFullscreen = FALSE;
    }

    return hr;
}


//
// Some video renderers support stepping media frame by frame with the
// IVideoFrameStep interface.  See the interface documentation for more
// details on frame stepping.
//
BOOL GetFrameStepInterface(void)
{
    HRESULT hr;
    IVideoFrameStep *pFSTest = NULL;

    // Get the frame step interface, if supported
    hr = pGB->QueryInterface(__uuidof(IVideoFrameStep), (PVOID *)&pFSTest);
    if (FAILED(hr))
        return FALSE;

    // Check if this decoder can step
    hr = pFSTest->CanStep(0L, NULL);

    if (hr == S_OK)
    {
        pFS = pFSTest;  // Save interface to global variable for later use
        return TRUE;
    }
    else
    {
        pFSTest->Release();
        return FALSE;
    }
}


HRESULT StepOneFrame(void)
{
    HRESULT hr=S_OK;

    // If the Frame Stepping interface exists, use it to step one frame
    if (pFS)
    {
        // The graph must be paused for frame stepping to work
        if (g_psCurrent != State_Paused)
            PauseClip();

        // Step the requested number of frames, if supported
        hr = pFS->Step(1, NULL);
    }

    return hr;
}


HRESULT StepFrames(int nFramesToStep)
{
    HRESULT hr=S_OK;

    // If the Frame Stepping interface exists, use it to step frames
    if (pFS)
    {
        // The renderer may not support frame stepping for more than one
        // frame at a time, so check for support.  S_OK indicates that the
        // renderer can step nFramesToStep successfully.
        if ((hr = pFS->CanStep(nFramesToStep, NULL)) == S_OK)
        {
            // The graph must be paused for frame stepping to work
            if (g_psCurrent != State_Paused)
                PauseClip();

            // Step the requested number of frames, if supported
            hr = pFS->Step(nFramesToStep, NULL);
        }
    }

    return hr;
}


HRESULT ModifyRate(double dRateAdjust)
{
    HRESULT hr=S_OK;
    double dRate;

    // If the IMediaPosition interface exists, use it to set rate
    if ((pMP) && (dRateAdjust != 0))
    {
        if ((hr = pMP->get_Rate(&dRate)) == S_OK)
        {
            // Add current rate to adjustment value
            double dNewRate = dRate + dRateAdjust;
            hr = pMP->put_Rate(dNewRate);

            // Save global rate
            if (SUCCEEDED(hr))
            {
                g_PlaybackRate = dNewRate;
            }
        }
    }

    return hr;
}


HRESULT SetRate(double dRate)
{
    HRESULT hr=S_OK;

    // If the IMediaPosition interface exists, use it to set rate
    if (pMP)
    {
        hr = pMP->put_Rate(dRate);

        // Save global rate
        if (SUCCEEDED(hr))
        {
            g_PlaybackRate = dRate;
        }
    }

    return hr;
}

double GetTime()
{
	HRESULT hr=S_OK;
	if (!pMP)   return 0;
	REFTIME time;
	hr = pMP->get_CurrentPosition(&time);
	if (SUCCEEDED(hr))
	{
		return time;
	}
	return 0;
}

double GetDur()
{
	HRESULT hr=S_OK;
	if (!pMP)   return 0;
	REFTIME time;
	hr = pMP->get_Duration(&time);
	if (SUCCEEDED(hr))
	{
		return time;
	}
	return 0;
}

HRESULT _handleGraphEvent(void)
{
    LONG evCode;
	LONG_PTR evParam1, evParam2;
    HRESULT hr=S_OK;

    // Make sure that we don't access the media event interface
    // after it has already been released.
    if (!pME)
        return S_OK;

    // Process all queued events
    while(SUCCEEDED(pME->GetEvent(&evCode, &evParam1, &evParam2, 0)))
    {
        // Free memory associated with callback, since we're not using it
        hr = pME->FreeEventParams(evCode, evParam1, evParam2);

        // If this is the end of the clip, reset to beginning
        if(EC_COMPLETE == evCode)
        {
            LONGLONG pos=0;

            // Reset to first frame of movie
            hr = pMS->SetPositions(&pos, AM_SEEKING_AbsolutePositioning ,
                                   NULL, AM_SEEKING_NoPositioning);
            if (FAILED(hr))
            {
                // Some custom filters (like the Windows CE MIDI filter)
                // may not implement seeking interfaces (IMediaSeeking)
                // to allow seeking to the start.  In that case, just stop
                // and restart for the same effect.  This should not be
                // necessary in most cases.
                if (FAILED(hr = pMC->Stop()))
                {
                    Msg(TEXT("Failed(0x%08lx) to stop media clip!\r\n"), hr);
                    break;
                }

                if (FAILED(hr = pMC->Run()))
                {
                    Msg(TEXT("Failed(0x%08lx) to reset media clip!\r\n"), hr);
                    break;
                }
            }
        }
    }

    return hr;
}

int GrabImg(LPCWSTR filename)
{

	// Must Pause before using GetCurrentImage
	if(!pMC)return 0;
	pMC->Pause();

	// get width and height
	long height, width;

	pBV->get_VideoHeight(&height);
	pBV->get_VideoWidth(&width);

	long bufSize;
	long *imgData;
	HRESULT hr;

	/*
	The second value is NULL to resolve required buffer size.
	The required buffer size will be returned in variable "bufSize".
	*/
	hr = pBV->GetCurrentImage(&bufSize, NULL);
	if (FAILED(hr)) {
		printf("GetCurrentImage failed\n");
		return 1;
	}

	if (bufSize < 1) {
		printf("failed to get data size\n");
		return 1;
	}
	imgData = (long *)malloc(bufSize);

	// The data will be in DIB format
	pBV->GetCurrentImage(&bufSize, imgData);

	// save DIB file as Bitmap.
	// This sample saves image as bitmap to help
	// understanding the sample.
	HANDLE fh;
	BITMAPFILEHEADER bmphdr;
	BITMAPINFOHEADER bmpinfo;
	DWORD nWritten;

	memset(&bmphdr, 0, sizeof(bmphdr));
	memset(&bmpinfo, 0, sizeof(bmpinfo));

	bmphdr.bfType = ('M' << 8) | 'B';
	bmphdr.bfSize = sizeof(bmphdr) + sizeof(bmpinfo) + bufSize;
	bmphdr.bfOffBits = sizeof(bmphdr) + sizeof(bmpinfo);

	bmpinfo.biSize = sizeof(bmpinfo);
	bmpinfo.biWidth = width;
	bmpinfo.biHeight = height;
	bmpinfo.biPlanes = 1;
	bmpinfo.biBitCount = 32;

	fh = CreateFile(filename,
		GENERIC_WRITE, 0, NULL,
		CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
	WriteFile(fh, &bmphdr, sizeof(bmphdr), &nWritten, NULL);
	WriteFile(fh, &bmpinfo, sizeof(bmpinfo), &nWritten, NULL);
	WriteFile(fh, imgData, bufSize, &nWritten, NULL);
	CloseHandle(fh);

	free(imgData);
	return 0;

}