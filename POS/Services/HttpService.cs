using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;

namespace POS.Services
{
    /// <summary>
    /// A simple HTTP Server class.
    /// </summary>
    class HttpService
    {
        // The default read buffer size
        private const int BUFFER_SIZE = 256;

        // The default port to listen on
        private const string DEFAULT_PORT = "8088";

        // a socket listener instance
        private readonly StreamSocketListener _listener;

        // a dictionary of supported content types
        // keyed on the file extension.
        private IDictionary<string, string> _contentTypes;

        /// <summary>
        /// create an instance of a HttpService class./>
        /// </summary>
        public HttpService()
        {
            _listener = new StreamSocketListener();
            _contentTypes = new Dictionary<string, string>();

            // initialise the supported content types
            _contentTypes[".html"] = "text/html";
            _contentTypes[".css"] = "text/css";
            _contentTypes[".js"] = "application/javascript";
            _contentTypes[".png"] = "image/png";
            _contentTypes[".jpeg"] = "image/jpeg";
            _contentTypes[".mp3"] = "audio/mpeg3";
            _contentTypes[".mp4"] = "vidio/mpeg";

            // start the service listening
            StartService();
        }

        /// <summary>
        /// Start the HTTP Server
        /// </summary>
        private async void StartService()
        {
            // when a connection is recieved, process
            // the request.
            _listener.ConnectionReceived += (s, e) =>
            {
                ProcessRequestAsync(e.Socket);
            };

            // Bind the service to the default port.
            await _listener.BindServiceNameAsync(DEFAULT_PORT);
        }

        /// <summary>
        /// When a connection is recieved, process the request.
        /// </summary>
        /// <param name="socket">the incoming socket connection.</param>
        private async void ProcessRequestAsync(StreamSocket socket)
        {
            StringBuilder inputRequestBuilder = new StringBuilder();

            // Read all the request data.
            // (This is assuming it is all text data of course)
            using(var input = socket.InputStream)
            {
                var data = new Windows.Storage.Streams.Buffer(BUFFER_SIZE);
                uint dataRead = BUFFER_SIZE;

                while (dataRead == BUFFER_SIZE)
                {
                    await input.ReadAsync(data, BUFFER_SIZE,
                        InputStreamOptions.Partial);

                    var dataArray = data.ToArray();
                    var dataString = Encoding.UTF8.GetString(dataArray, 0, dataArray.Length);

                    inputRequestBuilder.Append(dataString);

                    dataRead = data.Length;
                }
            }


            using(var output = socket.OutputStream)
            {
                // extract the request string.
                var request = inputRequestBuilder.ToString();

                var requestMethod = request.Split('\n')[0];
                var requestParts = requestMethod.Split(' ');

                if (requestParts[0].CompareTo("GET") == 0)
                {
                    // process the request and write the response.
                    await WriteResponseAsync(requestParts[1], socket.OutputStream);
                }
            }
            
        }

        /// <summary>
        /// Write the HTTP response to the request out to the output
        /// stream on the socket.
        /// </summary>
        /// <param name="resourceName">The resource name to retrieve.</param>
        /// <param name="outputStream">The output stream to write to.</param>
        /// <returns>A task object.</returns>
        private async Task WriteResponseAsync(string resourceName, IOutputStream outputStream)
        {
            using(var writeStream = outputStream.AsStreamForWrite())
            {
                // check the extension is supported.
                var extension = Path.GetExtension(resourceName);
                
                if(_contentTypes.ContainsKey(extension))
                {
                    string contentType = _contentTypes[extension];
                
                    // read the local data.
                    var localFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                
                    var requestedFile = await localFolder.GetFileAsync("www" + resourceName.Replace('/', '\\'));
                    var fileStream = await requestedFile.OpenReadAsync();
                    var size = fileStream.Size;
           
                    // write out the HTTP headers.
                    var header = String.Format("HTTP/1.1 200 OK\n" +
                                             "Content-Type: {0}\n" +
                                             "Content-Length: {1}\n" +
                                             "Connection: close\n" +
                                             "\n",
                                             contentType,
                                             fileStream.Size);

                    var headerArray = Encoding.UTF8.GetBytes(header);

                    await writeStream.WriteAsync(headerArray, 0, headerArray.Length);
                    
                    // copy the requested file to the output stream.
                    await fileStream.AsStreamForRead().CopyToAsync(writeStream);
                }
                else
                {
                    // unrecognised file type, just handle as
                    // a not found.

                    var header = "HTTP/1.1 404 Not Found\n" +
                                 "Connection: close\n\n";
                    var headerArray = Encoding.UTF8.GetBytes(header);
                    await writeStream.WriteAsync(headerArray, 0, headerArray.Length);
                }

                await writeStream.FlushAsync();
            }
        }

    }
}
