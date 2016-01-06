using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
   public static class UnauthorizedReasons
    {
        private const string unthorizedRequest = "Unauthorized Request! ";
        public const string UnauthorizedMessage = unthorizedRequest;
        public const string NoAppKeyHeader = unthorizedRequest+"Misss Key";
        public const string AppKeyIsNotExists = unthorizedRequest + "Invalid Consumer Key.";
        public const string NoAuthorizationHeaderOrScheme = unthorizedRequest + "We Can't Get Your Authorization From Your Request Or Your Authorization Scheme is Wrong.";
        public const string CantBuildRepresentation =unthorizedRequest+ "We Can't Build Representation From Your Request.Did you miss something?";
        public const string RequestIsExpire = unthorizedRequest + "This Request is Expire!";
        public const string NoDateHeaderValue = unthorizedRequest + " No Date Value!";
        public const string WrongSignature =unthorizedRequest+ "Invalid Signature.";
        public const string MD5NotMatch = unthorizedRequest + "Invalid Content Hash";
        public const string ReplayAttack = "Detected ReplayAttack!This Request Has Performed Before.";//Server Got this Signature before.

    }
}
