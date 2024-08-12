using DevelopmentKit.Audio;
using DevelopmentKit.Patterns.Design.Creational;
using Godot;

namespace Game.Managers;

public partial class SoundManager : AbstractSingleton<SoundManager>
{
    private const string OrderSoundNotificationPath = "res://Assets/Own/Audios/CustomerNotification.mp3";
    private const string TruckSoundNotificationPath = "res://Assets/Own/Audios/TruckAnnoucement.wav";

    private AudioStreamPlayer3D customerReachedSoundNotification;
    private AudioStreamPlayer3D truckSoundNotification;

    public override void _Ready()
    {
        customerReachedSoundNotification = AudioHandler.LoadAudioStreamPlayer3D(OrderSoundNotificationPath);
        truckSoundNotification = AudioHandler.LoadAudioStreamPlayer3D(TruckSoundNotificationPath);

        AddChild(truckSoundNotification);
        AddChild(customerReachedSoundNotification);
    }

    public void PlayCustomerReachedNotification(Vector3 customerLocation)
    { AudioHandler.PlaySoundAtLocation(customerReachedSoundNotification, customerLocation); }

    public void PlayTruckSoundNotification(Vector3 truckLocation)
    { AudioHandler.PlaySoundAtLocation(truckSoundNotification, truckLocation); }
}
