export class ApiError extends Error {
    statusCode: number;
    // name: string;

    constructor(statusCode: number, name: string, message: string) {
        super(message); // Pass message to parent constructor
        this.statusCode = statusCode;
        this.name = name; // Set the name of the error
    }
}