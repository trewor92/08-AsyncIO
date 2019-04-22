using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;


namespace AsyncApplication
{
    public static class Tasks
    {
        


        /// <summary>
        /// Returns the content of required uris.
        /// Method has to use the synchronous way and can be used to compare the performace of sync \ async approaches-проход. 
        /// </summary>
        /// <param name="uris">Sequence of required uri</param>
        /// <returns>The sequence of downloaded url content</returns>
        public static IEnumerable<string> GetUrlContent(this IEnumerable<Uri> uris)
        {

           

            WebClient client = new WebClient();
            return uris.Select(x=> client.DownloadString(x));

            // TODO : Implement GetUrlContent
            //throw new NotImplementedException();
        }



        /// <summary>
        /// Returns the content of required uris.
        /// Method has to use the asynchronous way and can be used to compare the performace of sync \ async approaches. 
        /// 
        /// maxConcurrentStreams parameter should control the maximum of concurrent streams that are running at the same time (throttling). 
        /// </summary>
        /// <param name="uris">Sequence of required uri</param>
        /// <param name="maxConcurrentStreams">Max count of concurrent request streams</param>
        /// <returns>The sequence of downloaded url content</returns>
        public static IEnumerable<string> GetUrlContentAsync(this IEnumerable<Uri> uris, int maxConcurrentStreams)
        {
            // Semaphore sm = new Semaphore(maxConcurrentStreams, maxConcurrentStreams);
              
            var collectionTasks = uris.Select(async uri =>
            await new WebClient().DownloadStringTaskAsync(uri));

            
            var queue = new Queue<Task<string>>(collectionTasks.Take(maxConcurrentStreams));
            var initialQueueCount = queue.Count();

            var actualEnumerator = collectionTasks.Skip(maxConcurrentStreams).GetEnumerator();
                        
            while (actualEnumerator.MoveNext())
            {
                queue.Enqueue(actualEnumerator.Current);
                yield return queue.Dequeue().Result;
            }

            while (initialQueueCount >0)
            {
                initialQueueCount--;
                yield return queue.Dequeue().Result;
            }


            // TODO : Implement GetUrlContentAsync
            //throw new NotImplementedException();
        }

      
        /// <summary>
        /// Calculates MD5 hash of required resource.
        /// 
        /// Method has to run asynchronous. 
        /// Resource can be any of type: http page, ftp file or local file.
        /// </summary>
        /// <param name="resource">Uri of resource</param>
        /// <returns>MD5 hash</returns>
        public static async Task<string> GetMD5Async(this Uri resource)
        {
            var md5 = MD5.Create();
            var stream = await new WebClient().OpenReadTaskAsync(resource);
            byte[] buffer = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = await stream.ReadAsync(buffer, 0, 4096);
                if (bytesRead == 0) break;
                md5.TransformBlock(buffer, 0, bytesRead, null, 0);
            }


            md5.TransformFinalBlock(new byte[0], 0, 0);

            return BitConverter.ToString(md5.Hash).Replace("-", "").ToLowerInvariant();

            // TODO : Implement GetMD5Async
            //throw new NotImplementedException();
        }
    }

}
