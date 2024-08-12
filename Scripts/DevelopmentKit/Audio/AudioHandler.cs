using Godot;

namespace DevelopmentKit.Audio;

public static class AudioHandler
{
    private const string ErrorMessageFormat = "Error when trying to load audio stream from file path: '{0}'...";

    public static AudioStreamPlayer3D LoadAudioStreamPlayer3D(NodePath audioSourcePath)
    {
        try
        {
            return new AudioStreamPlayer3D()
            {
                Stream = ResourceLoader.Load<AudioStream>(audioSourcePath)
            };
        }
        catch
        {
            GD.PrintErr(string.Format(ErrorMessageFormat, audioSourcePath));

            throw;
        }
    }

    public static void PlaySoundAtLocation(AudioStreamPlayer3D streamPlayer, Vector3 worldLocation)
    {
        streamPlayer.GlobalTransform = new Transform3D(Basis.Identity, worldLocation);
        streamPlayer.Play();
    }
}
