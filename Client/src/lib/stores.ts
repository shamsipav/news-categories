import { writable } from 'svelte/store'

export const modal = writable<{ open: () => void, close: () => void }>(undefined)