// NPSMLib: die Medienwiedergabe von Windows. Nur das, was Glaciers Musikfeld benutzt.
#nullable enable

namespace NPSMLib
{
    public enum MediaPlaybackCommands
    {
        Play = 0, Pause = 1, PlayPauseToggle = 2, Stop = 3,
        Previous = 4, Next = 5,
    }

    public enum MediaPlaybackState
    {
        Unknown = 0, Closed = 1, Opened = 2, Changing = 3,
        Stopped = 4, Playing = 5, Paused = 6,
    }

    public sealed class MediaObjectInfo
    {
        public string Title { get; } = string.Empty;
        public string Artist { get; } = string.Empty;
        public string AlbumTitle { get; } = string.Empty;
        public string AlbumArtist { get; } = string.Empty;
    }

    public sealed class MediaPlaybackInfo
    {
        public MediaPlaybackState PlaybackState { get; }
    }

    public sealed class MediaPlaybackDataChangedArgs : EventArgs { }

    public sealed class MediaPlaybackDataSource
    {
        public event EventHandler<MediaPlaybackDataChangedArgs>? MediaPlaybackDataChanged;
        public MediaObjectInfo GetMediaObjectInfo() => new();
        public MediaPlaybackInfo GetMediaPlaybackInfo() => new();
        public void SendMediaPlaybackCommand(MediaPlaybackCommands command) { }
    }

    public sealed class NowPlayingSession
    {
        public string SourceAppId { get; } = string.Empty;
        public MediaPlaybackDataSource ActivateMediaPlaybackDataSource() => new();
    }

    public sealed class NowPlayingSessionManagerEventArgs : EventArgs { }

    public sealed class NowPlayingSessionManager
    {
        public event EventHandler<NowPlayingSessionManagerEventArgs>? SessionListChanged;
        public NowPlayingSession[] GetSessions() => Array.Empty<NowPlayingSession>();
        public NowPlayingSession? CurrentSession { get; }
    }
}
