using System;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace NoteBookApp.Client.Pages
{
    public abstract class BoxBase : ComponentBase
    {
        [Inject]
        private NotificationService NotificationService { get; set; } = null!;

        protected void ShowError(string message, Exception? ex=null)
        {
            var msg = new NotificationMessage();
            msg.Severity = NotificationSeverity.Error;
            msg.Summary = message;
            NotificationService.Notify(msg);
        }
    }
    public abstract class PageBase: BoxBase
    {
        
        [Inject]
        protected DialogService DialogService { get; set; } = null!;

        
    }
}