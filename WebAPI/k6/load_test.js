import http from 'k6/http';
import { sleep } from 'k6';
export const options = {
    vus: 400,
    duration: '30s',
};
export default function () {
    http.get('https://localhost:7237/api/v1/employees');
    sleep(1);
}
