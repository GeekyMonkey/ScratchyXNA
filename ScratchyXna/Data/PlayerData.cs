using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;

namespace ScratchyXna
{
    public class PlayerData
    {
        private Dictionary<string, string> data = new Dictionary<string,string>();

        public string DataFileName = "Player.dat";

        private const char KeyValDelimeter = '=';
        private const char ItemDelimeter = '|';

        /// <summary>
        /// Empty all of the user data
        /// </summary>
        public void ClearAll()
        {
            data.Clear();
        }

        /// <summary>
        /// Set a value into the player data
        /// </summary>
        /// <param name="key">Key for the data</param>
        /// <param name="value">The value to save, or NULL to delete the value</param>
        public void SetValue(string key, object value)
        {
            if (value == null)
            {
                if (data.ContainsKey(key))
                {
                    data.Remove(key);
                }
            }
            else
            {
                data[key] = value.ToString();
            }
        }

        /// <summary>
        /// Get a string value from user data
        /// </summary>
        /// <param name="key">Key for the data</param>
        /// <param name="defaultValue">Default value if nothing is found</param>
        /// <returns>User value, or default value</returns>
        public string GetString(string key, string defaultValue)
        {
            if (data.ContainsKey(key))
            {
                return data[key];
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get an int value from user data
        /// </summary>
        /// <param name="key">Key for the data</param>
        /// <param name="defaultValue">Default value if nothing is found</param>
        /// <returns>User value, or default value</returns>
        public int GetInt(string key, int defaultValue)
        {
            try
            {
                return Int32.Parse(GetString(key, defaultValue.ToString()));
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get a bool value from user data
        /// </summary>
        /// <param name="key">Key for the data</param>
        /// <param name="defaultValue">Default value if nothing is found</param>
        /// <returns>User value, or default value</returns>
        public bool GetBool(string key, bool defaultValue)
        {
            try
            {
                return bool.Parse(GetString(key, defaultValue.ToString()));
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get a float value from user data
        /// </summary>
        /// <param name="key">Key for the data</param>
        /// <param name="defaultValue">Default value if nothing is found</param>
        /// <returns>User value, or default value</returns>
        public float GetFloat(string key, float defaultValue)
        {
            try
            {
                return float.Parse(GetString(key, defaultValue.ToString()));
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Save the user data
        /// </summary>
        public void Save()
        {
#if WINDOWS_PHONE || XBOX
            IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();
#else
            IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForDomain();
#endif

            StringBuilder dataString = new StringBuilder();
            foreach (KeyValuePair<string, string> keyval in data)
            {
                dataString.Append(keyval.Key);
                dataString.Append(KeyValDelimeter);
                dataString.Append(keyval.Value);
                dataString.Append(ItemDelimeter);
            }
            // Remove the last item delimeter
            if (dataString.Length > 0)
            {
                dataString.Remove(dataString.Length - 1, 1);
            }

            IsolatedStorageFileStream fs;
            using (fs = savegameStorage.CreateFile(DataFileName))
            {
                if (fs != null)
                {
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(dataString.ToString());
                    fs.Write(buffer, 0, buffer.Length);
                }
            }
        }

        /// <summary>
        /// Load the user data
        /// </summary>
        public void Load()
        {
#if WINDOWS_PHONE || XBOX
            IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();
#else
            IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForDomain();
#endif

            ClearAll();
            if (savegameStorage.FileExists(DataFileName))
            {
                using (IsolatedStorageFileStream fs = savegameStorage.OpenFile(DataFileName, System.IO.FileMode.Open))
                {
                    if (fs != null)
                    {
                        System.IO.StreamReader sr = new System.IO.StreamReader(fs);
                        string fileString = sr.ReadToEnd();
                        foreach(string item in fileString.Split(ItemDelimeter))
                        {
                            if (item == "")
                            {
                                break;
                            }
                            string[] keyval = item.Split(KeyValDelimeter);
                            data[keyval[0]] = keyval[1];
                        }
                    }
                }
            }
        }
    }
}
