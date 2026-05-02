
export class AuthUtils {
    constructor() { }
    static decode_token(token: string): any {
        const parts = token.split('.');
        if (parts.length !== 3) {
            throw new Error("Token is invalid. Check https://jwt.io")
        }
        const decode = this.decode_base64_url(parts[1]);
        return decode;
    }

    static decode_base64_url(base64Url: string): any {
        // JWT uses base64url: replace URL-safe chars back to standard base64
        debugger
        const base64 = base64Url
            .replace(/-/g, '+')
            .replace(/_/g, '/');

        // Pad to a multiple of 4
        const padded = base64 + '==='.slice((base64.length + 3) % 4);

        const jsonStr = decodeURIComponent(
            atob(padded)
                .split('')
                .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
                .join('')
        );


        return JSON.parse(jsonStr);
    }

    static getExpireDate(token: string): Date | null {
        try {
            const payload = this.decode_token(token);
            if (!payload.exp) return null;
            // JWT exp is in seconds since epoch
            return new Date(payload.exp * 1000);
        } catch {
            return null;
        }
    }

    static isTokenExpired(token: string): boolean {
        const exp = this.getExpireDate(token);
        if (!exp) return true;
        return exp < new Date();
    }
}