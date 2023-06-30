using System;
using System.IO;
using System.Threading.Tasks;

namespace S3Uploader
{
    public interface IFilesystemIo
    {
        Task<BucketResponse> Write(FilesystemData filesystemData);
    }

    public class FilesystemData
    {
        public string? Name { get; }
        public Stream? Data { get; }

        public FilesystemData(string? name, Stream? data)
        {
            Name = name;
            Data = data;
        }
    }

    public class FilesystemDataBuilder
    {
        private string? _name;
        private Stream? _stream;

        public FilesystemDataBuilder Name(string? name)
        {
            _name = name;
            return this;
        }

        public FilesystemDataBuilder Stream(Stream? stream)
        {
            _stream = stream;
            return this;
        }

        public FilesystemData Build()
        {
            if (_name == null)
            {
                throw new NameNotInitializedException();
            }

            if (_stream == null)
            {
                throw new StreamNotInitalizedException();
            }

            return new FilesystemData(_name, _stream);
        }
    }

    public class StreamNotInitalizedException : Exception
    {
    }

    public class NameNotInitializedException : Exception
    {
    }
}