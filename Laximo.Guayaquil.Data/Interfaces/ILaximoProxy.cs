using System;
using System.Security.Cryptography.X509Certificates;

namespace Laximo.Guayaquil.Data.Interfaces
{
    public interface ILaximoProxy : IDisposable
    {
        string QueryData(string query);
        X509CertificateCollection ClientCertificates { get; }
    }
}