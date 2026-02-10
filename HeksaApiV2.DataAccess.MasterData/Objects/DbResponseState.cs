using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.DataAccess.MasterData.Objects
{
    /// <summary>
    /// Status Code Dictionary(Add here if add new Status code)
    /// (
    /// 100 => Continue
    /// 200 => Success process,
    /// 204 => NoContent
    /// 500 => Error when raised error exception,
    /// 506 => Error parameter,
    /// 507 => All process success but expected goal didn't achieved and not raised eception error
    /// )
    /// </summary>
    public enum DbResponseState
    {
        /// <summary>
        /// Equivalent to HTTP status 100. System.Net.HttpStatusCode.Continue indicates that
        /// the client can continue with its request.
        /// </summary>
        Continue = 100,

        /// <summary>
        /// Equivalent to HTTP status 200. System.Net.HttpStatusCode.OK indicates that the
        /// request succeeded and that the requested information is in the response. This
        /// is the most common status code to receive.
        /// </summary>
        Ok = 200,

        /// <summary>
        /// Equivalent to HTTP status 204. System.Net.HttpStatusCode.NoContent indicates
        /// that the request has been successfully processed and that the response is intentionally
        /// blank.
        /// </summary>
        NoContent = 204,

        /// <summary>
        /// All process success but expected goal didn't achieved and not raised eception error
        /// </summary>
        ErrorButSuccess = 207,

        /// <summary>
        /// Equivalent to HTTP status 400. System.Net.HttpStatusCode.BadRequest indicates
        /// that the request could not be understood by the server. System.Net.HttpStatusCode.BadRequest
        /// is sent when no other error is applicable, or if the exact error is unknown or
        /// does not have its own error code.
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// Equivalent to HTTP status 401. System.Net.HttpStatusCode.Unauthorized indicates
        /// that the requested resource requires authentication. The WWW-Authenticate header
        /// contains the details of how to perform the authentication.
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// Equivalent to HTTP status 403. System.Net.HttpStatusCode.Forbidden indicates
        /// that the server refuses to fulfill the request.
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// Equivalent to HTTP status 404. System.Net.HttpStatusCode.NotFound indicates that
        /// the requested resource does not exist on the server.
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// Equivalent to HTTP status 500. System.Net.HttpStatusCode.InternalServerError
        /// indicates that a generic error has occurred on the server and raised an error exception
        /// </summary>
        InternalServerError = 500,

        /// <summary>
        /// Equivalent to HTTP status 501. System.Net.HttpStatusCode.NotImplemented indicates
        /// that the server does not support the requested function.
        /// </summary>
        NotImplemented = 501,

        /// <summary>
        /// Equivalent to HTTP status 502. System.Net.HttpStatusCode.BadGateway indicates
        /// that an intermediate proxy server received a bad response from another proxy
        /// or the origin server.
        /// </summary>
        BadGateway = 502,

        /// <summary>
        /// Equivalent to HTTP status 503. System.Net.HttpStatusCode.ServiceUnavailable indicates
        /// that the server is temporarily unavailable, usually due to high load or maintenance.
        /// </summary>
        ServiceUnavailable = 503,

        /// <summary>
        /// Equivalent to HTTP status 504. System.Net.HttpStatusCode.GatewayTimeout indicates
        /// that an intermediate proxy server timed out while waiting for a response from
        /// another proxy or the origin server.
        /// </summary>
        GatewayTimeout = 504,

        /// <summary>
        /// Equivalent to HTTP status 505. System.Net.HttpStatusCode.HttpVersionNotSupported
        /// indicates that the requested HTTP version is not supported by the server.
        /// </summary>
        HttpVersionNotSupported = 505,

        /// <summary>
        /// Error process return of validation parameter / or parameter not as expected
        /// </summary>
        ErrorParameter = 506
    }
}
