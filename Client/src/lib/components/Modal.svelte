<script lang="ts">
    import { onMount, createEventDispatcher } from 'svelte'
    import { fade, blur } from 'svelte/transition'
    import { clickOutside } from '$lib/actions'
    
    export let closable = true
    export let hasFooter = true
    export let title = 'Modal'

    let ready = false
    let visible = false
    const TRANSITION_DELAY = 50
    const TRANSITION_DURATION = 50
    const dispatch = createEventDispatcher()

    export const open = () => {
        if (visible) return
        visible = true
        setTimeout(() => ready = true, (TRANSITION_DELAY + TRANSITION_DURATION))
        dispatch('open')
    }

    export const close = () => {
        if (!visible) return
        if (closable && ready) {
            visible = false
            setTimeout(() => ready = false, (TRANSITION_DELAY + TRANSITION_DURATION))
            dispatch('close')
        }
    }

    export const toggle = () => visible ? close() : open()

    onMount(() => {
        document.addEventListener('keyup', (event: KeyboardEvent) => {
            if (event.key == 'Escape') {
                close()
            }
        })
    })
</script>

{ #if visible }
    <div class="modal" in:fade="{{ duration: TRANSITION_DURATION }}" out:fade="{{ delay: TRANSITION_DELAY, duration: TRANSITION_DURATION }}">
        <div class="modal-dialog modal-dialog-centered ">
            <div class="modal-content" use:clickOutside={ close }>
                <div class="modal-header">
                    <h5 class="modal-title">{ title }</h5>
                    <button
                        type="button"
                        class="btn-close"
                        on:click={ close }
                    />
                </div>
                <div class="modal-body">
                    <slot />
                </div>
                {#if hasFooter}
                    <div class="modal-footer">
                        <button
                            type="button"
                            class="btn btn-secondary"
                            on:click={ close }
                            >Close</button>
                        <button type="button" class="btn btn-primary">Save changes</button>
                    </div>
                {/if}
            </div>
        </div>
    </div>
{/if}

<style>
    .modal{
        display: block;
        background-color: rgba(125,125,125,0.3);
    }
</style>
