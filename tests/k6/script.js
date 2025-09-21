import http from 'k6/http';
import { sleep } from 'k6';

export default function () {
  const targetUrl = __ENV.TARGET_URL;
  if (!targetUrl) {
    throw new Error('TARGET_URL environment variable is not set.');
  }
  http.get(`${targetUrl}/api/TodoItems`);
  sleep(1);
}