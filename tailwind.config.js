/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml',
        './Areas/**/*.cshtml',
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
            'backgroundc': {
                '50': '#ffffff',
                '100': '#ffffff',
                '200': '#fcfcfc',
                '300': '#fcfcfc',
                '400': '#f7f8fa',
                '500': '#f6f6f8',
                '600': '#c5c5de',
                '700': '#8a8aba',
                '800': '#575794',
                '900': '#323270',
                '950': '#151547'
            },
            'primary': '#6F5BA9',
            'primaryc': {
                '50': '#f8f5fa',
                '100': '#f2ebf7',
                '200': '#dfd1eb',
                '300': '#c7b4db',
                '400': '#9b84c2',
                '500': '#6f5ba9',
                '600': '#5c4a96',
                '700': '#42327d',
                '800': '#2f2166',
                '900': '#1d124d',
                '950': '#0f0830'
            },
            'secondary': '#ad9fdb',
            'secondaryc': {
                '50': '#fbfafc',
                '100': '#f9f5fc',
                '200': '#efe6f7',
                '300': '#e2d5f0',
                '400': '#c9bae6',
                '500': '#ad9fdb',
                '600': '#9082c4',
                '700': '#685aa3',
                '800': '#483a85',
                '900': '#2c2163',
                '950': '#150d40'
            },
            'accent': '#8268d9',
            'accentc': {
                '50': '#f9f5fc',
                '100': '#f7f0fc',
                '200': '#e5d5f5',
                '300': '#d5bdf0',
                '400': '#b093e6',
                '500': '#8268d9',
                '600': '#6d54c4',
                '700': '#503ba3',
                '800': '#372682',
                '900': '#221561',
                '950': '#110940'
            },
            'darktextcolor': '#ededee',
            'darkbackground': '#070709',
            'darkbackgroundc': {
                '50': '#f1f0f2',
                '100': '#e3e1e6',
                '200': '#bcb8c2',
                '300': '#94909e',
                '400': '#494854',
                '500': '#070709',
                '600': '#07070a',
                '700': '#040408',
                '800': '#020205',
                '900': '#020205',
                '950': '#010103'
            },
            'darkprimary': '#6a56a4',
            'darkprimaryc': {
                '50': '#f8f5fa',
                '100': '#f0e9f5',
                '200': '#dbcce8',
                '300': '#c6b2db',
                '400': '#967ebf',
                '500': '#6a56a4',
                '600': '#584694',
                '700': '#40307a',
                '800': '#2b1e61',
                '900': '#1b114a',
                '950': '#0e0730'
            },
            'darksecondary': '#322460',
            'darksecondaryc': {
                '50': '#f4f0f7',
                '100': '#eae1f0',
                '200': '#c8b6d9',
                '300': '#a68fbf',
                '400': '#685191',
                '500': '#322460',
                '600': '#291d57',
                '700': '#1e1447',
                '800': '#150d3b',
                '900': '#0d072b',
                '950': '#07031c'
            },
            'darkaccent': '#402697',
            'darkaccentc': {
                '50': '#f6f0fa',
                '100': '#eee4f5',
                '200': '#d0bae6',
                '300': '#b496d6',
                '400': '#7857b5',
                '500': '#402697',
                '600': '#361f87',
                '700': '#271570',
                '800': '#1b0d59',
                '900': '#120845',
                '950': '#09032b'
            }
        },
    },
    plugins: [
        require('flowbite/plugin'),
        require('@tailwindcss/forms')

    ],
    darkMode: 'class',
}