using Microsoft.AspNetCore.Components;
using System;

namespace Blazored.Modal.Services
{
    public class ModalService : IModalService
    {
        private Type _modalType;

        public event Action<ModalResult> OnClose;
        
        internal event Action CloseModal;
        
        internal event Action<RenderFragment, ModalParameters> OnShow;

        public void Cancel()
        {
            CloseModal?.Invoke();
            OnClose?.Invoke(ModalResult.Cancel(_modalType));
        }

        public void Close() => Close(ModalResult.Ok<object>(null));

        public void Close(ModalResult modalResult)
        {
            modalResult.ModalType = _modalType;
            CloseModal?.Invoke();
            OnClose?.Invoke(modalResult);
        }

        public void Show<T>() where T : ComponentBase
        {
            Show<T>(new ModalParameters());
        }

        public void Show<T>(ModalParameters parameters) where T : ComponentBase
        {
            Show(typeof(T), parameters);
        }

        public void Show(Type contentComponent)
        {
            Show(contentComponent, new ModalParameters());
        }

        public void Show(Type contentComponent, ModalParameters parameters)
        {
            if (!typeof(ComponentBase).IsAssignableFrom(contentComponent))
            {
                throw new ArgumentException($"{contentComponent.FullName} must be a Blazor Component");
            }

            var content = new RenderFragment(x => { x.OpenComponent(1, contentComponent); x.CloseComponent(); });
            _modalType = contentComponent;

            OnShow?.Invoke(content, parameters);
        }
    }
}
