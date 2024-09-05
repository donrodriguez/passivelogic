export type Success<T> = {
    isSuccess: true;
    value: T;
};

export type Failure<E> = {
    isSuccess: false;
    error: E;
};

export type FailureResponse<E, R> = {
    isSuccess: false;
    error: E;
    value: R;
};

export type Result<T, E> = Success<T> | Failure<E>;

export type ResultResponse<T, E, R> = Success<T> | FailureResponse<E, R>;

export function success<T>(value: T): Success<T> {
    return { isSuccess: true, value };
}

export function failure<E>(error: E): Failure<E> {
    return { isSuccess: false, error };
}

export function failureResponse<E, R>(error: E, value: R): FailureResponse<E, R> {
    return { isSuccess: false, error, value };
}
