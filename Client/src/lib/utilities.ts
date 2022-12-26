import { ajax } from 'jquery'
import type { RESTMethod, DefaultAJAXResponse, ContentType } from '$lib/types'

export const postData = async (url = '', data = {}) => {
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
    return response.json()
}

const redirectDelay = 500
export const redirect = (location: string) => setTimeout(() => { window.location.href = location }, redirectDelay)

export async function logout() {
    const response = await fetch('http://localhost:8080/api/auth/logout')

    if (response.ok) {
        redirect('/')
        console.log('LOGOUT SUCCESS', response)

    } else {
        console.log('Ошибка HTTP: ' + response.status)
    }
}

interface AJAXOptions {
    method: RESTMethod,
    contentType?: ContentType,
    data?: FormData | Record<string, string> | null,
    csrfToken?: string,
    headers?: JQuery.PlainObject<string> | null
}

// Create a plain JSON from FormData
export function transformFormData(form: FormData): Record<string, unknown> {
    const object: Record<string, unknown> = {}
    form.forEach((value, key) => object[key] = value)
    return object
}

// Функция для отправки AJAX запросов с клиента
export const sendWindowAJAX = (
    url: string,
    options: AJAXOptions = { method: 'GET', contentType: 'application/x-www-form-urlencoded' },
    callbackSuccess?: (res: DefaultAJAXResponse) => void,
    callbackError?: (res: string) => void
) => {
    let finalData: Record<string, unknown> | FormData

    if (!options.contentType) options.contentType = 'application/x-www-form-urlencoded'

    if (options.data instanceof FormData && (options.contentType === 'application/x-www-form-urlencoded' || options.contentType === 'application/json')) {
        // finalData = transformFormData(options.data)
        finalData = Object.fromEntries(options.data.entries());

        if (options.data.getAll("categories").length) {
            let categories = options.data.getAll("categories");
            let names = options.data.getAll("names");

            const categoriesObject = []

            for (let i = 0; i <= categories.length; i++) {
                const id = categories[i]
                const name = names[i]
                if (id && name) {
                    categoriesObject[i] = {
                        id: id,
                        name: name,
                        news: []
                    }
                }
            }

            finalData.categories = categoriesObject
        }

        if (options.data.getAll("news").length) {
            const newsObject = []
            finalData.news = newsObject
        }

        console.log(finalData)

        if (options.csrfToken) finalData.csrf = options.csrfToken
    }
    else if (options.data && options.data instanceof FormData) {
        finalData = options.data
        if (options.csrfToken) finalData.set('csrf', options.csrfToken)
    }

    const request = ajax({
        url: url,
        contentType: options.contentType === 'multipart/form-data' ? false : options.contentType,
        headers: options.headers || {},
        type: options.method,
        data: options.contentType === 'application/json' ? JSON.stringify(finalData) : finalData,
        dataType: 'json',
        processData: options.contentType === 'multipart/form-data' ? false : true
    })

    console.log(request)

    request.done((res) => {
        if (res.ok === true) {
            if (callbackSuccess) callbackSuccess(res)
        }
        else if (res.ok === false) {
            if (callbackError) callbackError(res)
            console.error(res)
        }
        else {
            if (callbackSuccess) callbackSuccess(res)
        }
    })

    request.fail((jqXHR) => {
        if (callbackError) callbackError(
            (jqXHR.responseJSON && jqXHR.responseJSON.error) ? jqXHR.responseJSON.error : jqXHR.responseText
        )
    })
}

export const clickOutside = (element: Element, callbackFunction: () => void) => {
    function onClick(event: Event) {
        if (!element.contains(event.target as Node)) {
            callbackFunction()
        }
    }

    document.body.addEventListener('click', onClick)

    return {
        update(newCallbackFunction: () => void) {
            callbackFunction = newCallbackFunction
        },
        destroy() {
            document.body.removeEventListener('click', onClick)
        }
    }
}
