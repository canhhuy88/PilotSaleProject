export function mapApiError(error: any): { errorMessage: string } {
  const status = error?.response?.status;
  const data = error?.response?.data;

  if (status === 400) {
    if (typeof data === 'string') {
      return { errorMessage: 'Invalid username or password' };
    } else if (data && data.message) {
      return { errorMessage: data.message };
    } else {
      return { errorMessage: 'Bad request' };
    }
  }

  if (status === 401) {
    return { errorMessage: 'Unauthorized' };
  }

  if (status === 403) {
    return { errorMessage: 'Forbidden' };
  }

  if (status === 500) {
    return { errorMessage: 'Server error' };
  }

  return { errorMessage: 'Something went wrong' };
}
