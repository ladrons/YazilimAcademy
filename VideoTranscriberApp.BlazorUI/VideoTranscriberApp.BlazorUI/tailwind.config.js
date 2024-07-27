/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ['./**/*.{razor,html,cshtml}', '../VideoTranscriberApp.BlazorUI.Client/**/*.{razor,html,cshtml}'],
    theme: {
        extend: {

        },
    },
    plugins: [
        require('daisyui'),
        require('@tailwindcss/forms'),
    ],
    daisyui: {
        themes: ["light", "dark", "synthwave"],
    },
}