using System;

[Serializable]
public class FrameManager
{
    public int FrameNb;
    public int Fps;
    public int MaxFrameNb;
    public int FrameNbToStopIfTransition;
    public int FrameNbToStartIfTransition;
    
    public int PoseFrameNb;
    public int PoseFps;
    public int PoseMaxFrameNb;
    public int PoseFrameNbToStopIfTransition;
    public int PoseFrameNbToStartIfTransition;
    
    public void Init(Frame[] frames, Frame[] poseFrames)
    {
        MaxFrameNb = frames.Length;
        
        PoseMaxFrameNb = poseFrames.Length;
        CalculateFpsFromFrameTab(frames, poseFrames);
        CalculateFramesToStopAndStartIfTransition();
        FrameNb = 0;
        PoseFrameNb = 0;
    }
    
    public void Init(Frame[] frames)
    {
        MaxFrameNb = frames.Length;
        CalculateFpsFromFrameTab(frames);
        CalculateFramesToStopAndStartIfTransition();
        FrameNb = 0;
    }

    private void CalculateFramesToStopAndStartIfTransition()
    {
        float transitionDurationRef = MainManager.Instance.TransitionDuration;

        FrameNbToStartIfTransition = (int)Math.Round(Fps * transitionDurationRef * 0.5, MidpointRounding.AwayFromZero);
        FrameNbToStopIfTransition = (MaxFrameNb - FrameNbToStartIfTransition);
        
        PoseFrameNbToStartIfTransition = (int)Math.Round(PoseFps * transitionDurationRef * 0.5, MidpointRounding.AwayFromZero);
        PoseFrameNbToStopIfTransition = (PoseMaxFrameNb - PoseFrameNbToStartIfTransition);
        
    }
    
    public void CalculateFpsFromFrameTab(Frame[] frames)
    {
        if (frames.Length > 1)
        {
            double timeOne = frames[0].Timestamp;
            double timeTwo = frames[1].Timestamp;

            Fps = (int) (1 / (timeTwo - timeOne));
        }
    }
    
    /// <summary>
    /// Calculates the frames per second (FPS) based on an array of frames.
    /// </summary>
    /// <param name="frames">The array of frames.</param>
    private void CalculateFpsFromFrameTab(Frame[] frames, Frame[] poseFrames)
    {
        if (frames.Length > 1)
        {
            double timeOne = frames[0].Timestamp;
            double timeTwo = frames[1].Timestamp;

            Fps = (int) (1 / (timeTwo - timeOne));
        }
        if(poseFrames.Length > 1)
        {
            double timeOne = poseFrames[0].Timestamp;
            double timeTwo = poseFrames[1].Timestamp;

            PoseFps = (int) (1 / (timeTwo - timeOne));
        }
        

        // Fix for OpenFace observed after several CSV imports : the clips are speeded up by 5 FPS (the timestamp >= 1.0d should happen 5 FPS earlier
        //Fps -= 5;
    }

    public void FrameExecuted(bool willTransitionToOtherAction)
    {
        if (willTransitionToOtherAction && FrameNb < FrameNbToStopIfTransition - 1)
        {
            FrameNb++;
        }
        else if (!willTransitionToOtherAction && FrameNb < MaxFrameNb - 1)
        {
            FrameNb++;
        }
        else
        {
            FrameNb = 0;
            MainManager.Instance.EndOfActionEvent();
        }
        
        if(willTransitionToOtherAction && PoseFrameNb < PoseFrameNbToStopIfTransition - 1)
        {
            PoseFrameNb++;
        }
        else if (!willTransitionToOtherAction && PoseFrameNb < PoseMaxFrameNb - 1)
        {
            PoseFrameNb++;
        }
        else
        {
            PoseFrameNb = 0;
            MainManager.Instance.EndOfActionEvent();
        }
        
    }

    public void SkipFramesForTransition()
    {
        FrameNb = FrameNbToStartIfTransition;
        PoseFrameNb = PoseFrameNbToStartIfTransition;
    }
}
