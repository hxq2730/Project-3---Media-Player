using SpotifyProject.Models;
using SpotifyProject.Services;
using WMPLib;

namespace SpotifyProject.ViewModels
{

    public class PlaylistPageVM
    {
        public Playlist Playlist { get; set; }
        public PlaylistService PlaylistService { get; set; }
        public MediaService MediaService { get; set; }

        public delegate void ChangeStateIconDelegate();
        public event ChangeStateIconDelegate ChangeStateIcon;

        public PlaylistPageVM(Playlist playlist)
        {
            Playlist = playlist;
            PlaylistService = new PlaylistService(DatabaseConnection.GetInstance().getConnection());
            MediaService = new MediaService(DatabaseConnection.GetInstance().getConnection());
        }

        public int AddSongToPlaylist(Song song)
        {
            int insertRow = MediaService.AddMediaItemSong(song, Playlist.Id);
            return insertRow;
        }
        public int AddVideoToPlaylist(Video video)
        {
            int insertRow = MediaService.AddMediaVideo(video, Playlist.Id);

            return insertRow;
        }

        public void LoadPlaylist()
        {
            Playlist = PlaylistService.GetPlaylist(Playlist.Id);
        }

        public void PlaySong(Song selectedSong)
        {
            WindowsMediaPlayer wmp = new WindowsMediaPlayer();
            IWMPMedia media = wmp.newMedia(selectedSong.Path);

            wmp.controls.stop();
            wmp.currentPlaylist.appendItem(media);
            wmp.controls.play();
        }
        

        // Add recents
        public void AddRecentFile(int idMedia, int idPlaylist)
        {
            MediaService.AddRecents(idMedia, idPlaylist);
        }

        // Delete media item
        public void DeleteMediaItem(int idMedia)
        {
            PlaylistService.DeleteMediaItemFromPlaylist(idMedia, Playlist.Id);
        }
       
    }
}
