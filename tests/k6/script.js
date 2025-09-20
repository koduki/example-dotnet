import http from 'k6/http';
import { sleep } from 'k6';

export default function () {
  http.get('https://example-dotnet-367762831588.asia-northeast1.run.app/api/TodoItems');
  sleep(1);
}