/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml',
        "./node_modules/flowbite/**/*.js"
    ],
    /*https://www.realtimecolors.com/?colors=ededee-070709-6a56a4-322460-402697&fonts=Poppins-Poppins*/
    theme: {
        extend: {},
        colors: {
            transparent: 'transparent',
            current: 'currentColor',
            'textcolor': '#111112',
            'background': '#F6F6F8',
            'primary': '#6F5BA9',
            'secondary': '#ad9fdb',
            'accent': '#8268d9',
            'darktextcolor': '#ededee',
            'darkbackground': '#070709',
            'darkprimary': '#6a56a4',
            'darksecondary': '#322460',
            'darkaccent': '#402697',
        },
    },
    plugins: [
        require('flowbite/plugin'),
        require('@tailwindcss/forms')

    ],
    darkMode: 'class',
}