using SpotifyProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SpotifyProject.Services
{
    public class MediaService
    {
        SQLiteConnection connection;
        public MediaService(SQLiteConnection conn)
        {
            connection = conn;
        }

        public int AddMediaItemSong(Song song, int idPlaylist)
        {
            try {
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "INSERT INTO MediaItems (Title, Artist, Type, Path, Album, Year, Genre, Length, PlaylistId) VALUES (@title, @artist, @type, @path, @album, @year, @genre, @length, @idPlaylist)";
                command.Parameters.AddWithValue("@title", song.Title);
                command.Parameters.AddWithValue("@artist", song.Artist);
                command.Parameters.AddWithValue("@type", song.Type);
                command.Parameters.AddWithValue("@path", song.Path);
                command.Parameters.AddWithValue("@album", song.Album);
                command.Parameters.AddWithValue("@year", song.Year);
                command.Parameters.AddWithValue("@genre", song.Genre);
                command.Parameters.AddWithValue("@length", song.Length);
                command.Parameters.AddWithValue("@idPlaylist", idPlaylist);

                int result = command.ExecuteNonQuery();
                return result;
            } catch(Exception err) {
                MessageBox.Show("Error adding song to playlist");
                return -1;
            }
            
        }

        public int AddMediaVideo(Video video, int idPlaylist)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "INSERT INTO MediaItems (Title, Artist, Type, Path, Album, Year, Genre, Length, PlaylistId) VALUES (@title, @artist, @type, @path, @album, @year, @genre, @length, @idPlaylist)";
                command.Parameters.AddWithValue("@title", video.Title);
                command.Parameters.AddWithValue("@artist", video.Artist);
                command.Parameters.AddWithValue("@type", video.Type);
                command.Parameters.AddWithValue("@path", video.Path);
                command.Parameters.AddWithValue("@album", "Unknown");
                command.Parameters.AddWithValue("@year", video.Date);
                command.Parameters.AddWithValue("@genre", "Unknown");
                command.Parameters.AddWithValue("@length", video.Length);
                command.Parameters.AddWithValue("@idPlaylist", idPlaylist);

                int result = command.ExecuteNonQuery();
                return result;
            }
            catch (Exception err)
            {
                MessageBox.Show("Error adding video to playlist");
                return -1;
            }
        }

        // get a media item by id
        public MediaItem GetMediaItemById(int id)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM MediaItems WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int idMediaItem = Convert.ToInt32(reader["Id"]);
                    string title = reader["Title"].ToString();
                    string artist = reader["Artist"].ToString();
                    string type = reader["Type"].ToString();
                    string path = reader["Path"].ToString();
                    string album = reader["Album"].ToString();
                    string year = reader["Year"].ToString();
                    string gerne = reader["Genre"].ToString();
                    string length = reader["Length"].ToString();

                    if (type == "0")
                    {
                        return new Song(idMediaItem, title, artist, album, year, gerne, length, path);
                    }
                    else
                    {
                        return new Video(idMediaItem, title, artist, path, year, length);
                    }
                }
                return null;
            }
            catch (Exception err)
            {
                MessageBox.Show("Error getting media item by id");
                return null;
            }
        }

        public List<MediaItem> GetListMediaItemsOfPlaylist(int idPlaylist)
        {
            List<MediaItem> listMediaItems = new List<MediaItem>();
            try
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM MediaItems WHERE PlaylistId = @idPlaylist";
                command.Parameters.AddWithValue("@idPlaylist", idPlaylist);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string title = reader["Title"].ToString();
                    string artist = reader["Artist"].ToString();
                    string type = reader["Type"].ToString();
                    string path = reader["Path"].ToString();
                    string album = reader["Album"].ToString();
                    string year = reader["Year"].ToString();
                    string gerne = reader["Genre"].ToString();
                    string length = reader["Length"].ToString();

                    if (type == "0")
                    {
                        listMediaItems.Add(new Song(id, title, artist, album, year, gerne, length, path));
                    } else
                    {
                        listMediaItems.Add(new Video(id, title, artist, path, year, length));
                    }
                }
                return listMediaItems;
            } catch(Exception err)
            {
                MessageBox.Show("Error getting list of media items");
                return listMediaItems;
            }
            
        }

       

        // load list recents
        public List<Recents> GetRecents()
        {
               List<Recents> listRecents = new List<Recents>();
            try
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM Recents";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    int idMediaItem = Convert.ToInt32(reader["MediaItemId"]);
                    int idPlaylist = Convert.ToInt32(reader["PlaylistId"]);
                    Recents recents = new Recents(id, idPlaylist, idMediaItem);
                    listRecents.Add(recents);
                }
                return listRecents;
            }
            catch (Exception err)
            {
                MessageBox.Show("Error getting list of recents");
                return listRecents;
            }
        }


        public int AddRecents(int idMediaItem, int idPlaylist)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "INSERT INTO Recents (MediaItemId, PlaylistId) VALUES (@idMediaItem, @idPlaylist)";
                command.Parameters.AddWithValue("@idMediaItem", idMediaItem);
                command.Parameters.AddWithValue("@idPlaylist", idPlaylist);

                int result = command.ExecuteNonQuery();
                return result;
            }
            catch (Exception err)
            {
                MessageBox.Show("Error adding recents");
                return -1;
            }
        }
    
        public int DeleteRecents(int idMediaItem, int idPlaylist)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "DELETE FROM Recents WHERE MediaItemId = @idMediaItem AND PlaylistId = @idPlaylist";
                command.Parameters.AddWithValue("@idMediaItem", idMediaItem);
                command.Parameters.AddWithValue("@idPlaylist", idPlaylist);

                int result = command.ExecuteNonQuery();
                return result;
            }
            catch (Exception err)
            {
                MessageBox.Show("Error deleting recents");
                return -1;
            }
        }
    }
}
