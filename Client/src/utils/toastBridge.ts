let toastHandler: ((message: string) => void) | null = null;

export function setToastHandler(handler: (message: string) => void) {
  toastHandler = handler;
}

export function showErrorToast(message: string) {
  if (toastHandler) {
    toastHandler(message);
  }
}
