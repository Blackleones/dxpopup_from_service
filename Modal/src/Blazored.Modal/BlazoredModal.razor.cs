using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;

namespace Blazored.Modal
{
    public partial class BlazoredModal : IDisposable
    {
        [Inject] private IModalService ModalService { get; set; }

        [Parameter] public bool HideHeader { get; set; }
        [Parameter] public bool HideCloseButton { get; set; }
        [Parameter] public bool DisableBackgroundCancel { get; set; }
        [Parameter] public string Position { get; set; }
        [Parameter] public string Style { get; set; }

        private RenderFragment Content { get; set; }
        private ModalParameters Parameters { get; set; }

        protected override void OnInitialized()
        {
            ((ModalService)ModalService).OnShow += ShowModal;
            ((ModalService)ModalService).CloseModal += CloseModal;
        }

        private async void ShowModal(RenderFragment content, ModalParameters parameters)
        {
            Content = content;
            Parameters = parameters;

            await InvokeAsync(StateHasChanged);
        }

        private async void CloseModal()
        {
            Content = null;
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ((ModalService)ModalService).OnShow -= ShowModal;
                ((ModalService)ModalService).CloseModal -= CloseModal;
            }
        }
    }
}
