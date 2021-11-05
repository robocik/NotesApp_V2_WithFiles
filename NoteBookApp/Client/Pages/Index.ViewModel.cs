using System;
using System.Collections.Generic;
using NoteBookApp.Shared;

namespace NoteBookApp.Client.Pages
{
    public class IndexViewModel
    {
        public IndexViewModel(MyProfileDto profile)
        {
            Email = profile.Email;

            ProfileUrl = profile.AvatarUrl;
        }

        public string? ProfileUrl { get; set; }
        public string Email { get; set; }
    }

}