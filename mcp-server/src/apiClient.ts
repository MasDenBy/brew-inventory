import http from "node:http";
import https from "node:https";
import { URL } from "node:url";

const API_BASE = process.env.BREW_INVENTORY_API || "https://localhost:7000";

function isLocalhost(hostname: string): boolean {
  return (
    hostname === "localhost" ||
    hostname === "127.0.0.1" ||
    hostname === "::1" ||
    hostname === "[::1]"
  );
}

interface RequestResult {
  status: number;
  body: string;
}

function httpRequest(urlStr: string, init: RequestInit, redirects: number): Promise<RequestResult> {
  return new Promise((resolve, reject) => {
    const url = new URL(urlStr);
    const isHttps = url.protocol === "https:";
    const lib = isHttps ? https : http;

    const req = lib.request(
      url,
      {
        method: init.method || "GET",
        headers: {
          "Content-Type": "application/json",
          ...(init.headers as Record<string, string> | undefined),
        },
        rejectUnauthorized: !isLocalhost(url.hostname),
      },
      (res) => {
        const chunks: Buffer[] = [];
        res.on("data", (c) => chunks.push(c));
        res.on("end", () => {
          const body = Buffer.concat(chunks).toString();
          const status = res.statusCode || 0;

          if (status >= 300 && status < 400 && res.headers.location) {
            const next = new URL(res.headers.location, url);
            if (redirects <= 0) {
              reject(new Error(`Too many redirects loading ${urlStr}`));
              return;
            }
            const preserve = status === 307 || status === 308;
            httpRequest(
              next.toString(),
              preserve ? init : { method: "GET" },
              redirects - 1,
            )
              .then(resolve)
              .catch(reject);
            return;
          }

          resolve({ status, body });
        });
      },
    );

    req.on("error", (e) => reject(new Error(`API request failed: ${e.message}`)));

    if (init.body) req.write(init.body as string);
    req.end();
  });
}

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const { status, body } = await httpRequest(`${API_BASE}${path}`, init || {}, 5);

  if (status < 200 || status >= 300) {
    throw new Error(`API error ${status}: ${body}`);
  }

  if (status === 204 || body.length === 0) return {} as T;
  return JSON.parse(body) as T;
}

export const api = {
  get: <T>(path: string) => request<T>(path),
  post: <T>(path: string, body: unknown) =>
    request<T>(path, { method: "POST", body: JSON.stringify(body) }),
};