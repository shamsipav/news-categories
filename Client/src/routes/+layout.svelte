<script lang="ts">
    import dayjs from 'dayjs'
    import 'dayjs/locale/ru'

    dayjs.locale('ru')

    import { page } from '$app/stores'
    $: URL = $page.url.pathname

    interface MenuItem {
        path: string,
        title: string
    }

    const menuItems: MenuItem[] = [
        { title: 'Главная страница', path: '/' },
        { title: 'Новости', path: '/news' },
        { title: 'Категории', path: '/categories' },
    ]
</script>

<div class="container">
    <header class="d-flex justify-content-center py-3">
        <ul class="nav nav-pills">
            {#each menuItems as item, i}
                <li class="nav-item"><a href={item.path} class="nav-link" class:active={ i == 0 ? URL == item.path : URL.startsWith(item.path) }>{item.title}</a></li>
            {/each}
        </ul>
    </header>
</div>

<main class="pt-5">
    <slot />
</main>
