﻿namespace SwEpApi.Model.Request
{
    public class LoginRequest : ModelBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }        
    }
}
