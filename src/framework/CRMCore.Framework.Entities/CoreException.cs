﻿using System;

namespace CRMCore.Framework.Entities
{
    public class CoreException : Exception
    {
        public CoreException(string message)
            : this(message, null)
        {
        }

        public CoreException(string message, Exception innerEx)
            : base(message, innerEx)
        {
        }
    }

    public class DomainException : CoreException
    {
        public DomainException(string message)
            : base(message, null)
        {
        }
    }

    public class ViolateSecurityException : CoreException
    {
        public ViolateSecurityException(string message)
            : base(message, null)
        {
        }
    }
}