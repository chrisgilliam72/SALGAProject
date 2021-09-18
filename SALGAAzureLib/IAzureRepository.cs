using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SALGAEvidenceRepository
{
    public interface IAzureRepository
    {
        public void SetConnectionString(String connectionString);
        public Task<String> AddFile(String containerName, String fileName, Stream file);
        public bool GetFile(String containerName, string fileName, Stream file);

        public Task<bool> DeleteFile(String containerName, string fileName);

        public Task<bool> UpdateMetaData(String containerName, String fileName, IDictionary<String, String> MetaData);
    }
}
