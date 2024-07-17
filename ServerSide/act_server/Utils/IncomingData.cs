namespace act_server.Utils;

public class Pose
{
    public double pose_Tx { get; set; }
    public double pose_Ty { get; set; }
    public double pose_Tz { get; set; }
    public double pose_Rx { get; set; }
    public double pose_Ry { get; set; }
    public double pose_Rz { get; set; }
}

public class Gaze
{
    public double gaze_angle_x { get; set; }
    public double gaze_angle_y { get; set; }
    public double gaze_0_x { get; set; }
    public double gaze_0_y { get; set; }
    public double gaze_0_z { get; set; }
    public double gaze_1_x { get; set; }
    public double gaze_1_y { get; set; }    
    public double gaze_1_z { get; set; }
}

public class AuC
{
    public double AU04 { get; set; }
    public double AU05 { get; set; }
    public double AU06 { get; set; }
    public double AU07 { get; set; }
    public double AU10 { get; set; }
    public double AU12 { get; set; }
    public double AU14 { get; set; }
    public double AU23 { get; set; }
    public double AU01 { get; set; }
    public double AU02 { get; set; }
    public double AU09 { get; set; }
    public double AU15 { get; set; }
    public double AU17 { get; set; }
    public double AU20 { get; set; }
    public double AU25 { get; set; }
    public double AU26 { get; set; }
    public double AU28 { get; set; }
    public double AU45 { get; set; }
}

public class AuR
{
    public double AU04 { get; set; }
    public double AU06 { get; set; }
    public double AU07 { get; set; }
    public double AU10 { get; set; }
    public double AU12 { get; set; }
    public double AU14 { get; set; }
    public double AU01 { get; set; }
    public double AU02 { get; set; }
    public double AU05 { get; set; }
    public double AU09 { get; set; }
    public double AU15 { get; set; }
    public double AU17 { get; set; }
    public double AU20 { get; set; }
    public double AU23 { get; set; }
    public double AU25 { get; set; }
    public double AU26 { get; set; }
    public double AU45 { get; set; }
}

public class IncomingData
{
    public double timestamp { get; set; }
    public int frame { get; set; }
    public double confidence { get; set; }
    public string roomId { get; set; }
    public Pose pose { get; set; }
    public Gaze gaze { get; set; }
    public AuC au_c { get; set; }
    public AuR au_r { get; set; }
}

public struct AudioIncomingData
{
    byte[] buffer;
    int bytesRecorded;
    
    
}
