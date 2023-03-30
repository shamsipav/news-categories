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

export function getCookie(name) {
    const cookieName = name + '='
    const decodedCookie = decodeURIComponent(document.cookie)
    const ca = decodedCookie.split(';')
    for(let i = 0; i < ca.length; i++) {
        let c = ca[i]
        while (c.charAt(0) == ' ') {
            c = c.substring(1)
        }
        if (c.indexOf(cookieName) == 0) {
            return c.substring(cookieName.length, c.length)
        }
    }
    return ''
}

export function setCookie(name, value, exdays, secure?) {
    const d = new Date()
    d.setTime(d.getTime() + (exdays*24*60*60*1000))
    const expires = 'expires='+ d.toUTCString()
    if (secure) {
        document.cookie = name + '=' + value + ';' + expires + ';path=/; secure'
    } else {
        document.cookie = name + '=' + value + ';' + expires + ';path=/'
    }
}

export function deleteCookie(name) {
    document.cookie = name+'=; Max-Age=-99999999;'
}

export async function logout() {
    setCookie('token', null, null)
    redirect('/')
}

export function objectHasKey(obj, findKey: string) {
    Object.keys(obj).map((key) => {
        if (key.startsWith(findKey)) {
            return true
        }
    })

    return false
}