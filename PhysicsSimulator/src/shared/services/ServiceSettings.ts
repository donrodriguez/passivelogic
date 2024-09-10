enum Scheme {
    HTTP = "http",
    HTTPS = "https"
}

class Configuration {
    static scheme: Scheme;
    static host: string;
    static port: string;
    static get baseUrl(): string {
        return `${this.scheme}://${this.host}:${this.port}`;
    }
}

// Assuming you have a way to determine if the environment is 'DEBUG'
const isDebug = process.env.NODE_ENV === 'development'; // or any other appropriate check
// const isDebug = true;
if (isDebug) {
    Configuration.scheme = Scheme.HTTP;
    Configuration.host = "localhost";
    Configuration.port = "5293";

} else {
    Configuration.scheme = Scheme.HTTP;
    Configuration.host = "localhost";
    Configuration.port = "5293";
}

function endpoint(path: string): string {
    return Configuration.baseUrl + path;
}

export { Configuration, endpoint };