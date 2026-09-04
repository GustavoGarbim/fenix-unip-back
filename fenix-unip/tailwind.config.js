/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  theme: {
    extend: {
      colors: {
        fenix: {
          black: '#0a0a0a',
          ember: '#1a0f0a',
          red: '#c81e1e',
          crimson: '#8b0000',
          orange: '#f97316',
          gold: '#facc15',
        },
      },
      fontFamily: {
        display: ['"Anton"', '"Oswald"', 'sans-serif'],
        sans: ['"Inter"', 'system-ui', 'sans-serif'],
      },
      backgroundImage: {
        'fire-gradient': 'linear-gradient(135deg, #8b0000 0%, #c81e1e 35%, #f97316 70%, #facc15 100%)',
        'radial-ember': 'radial-gradient(ellipse at center, rgba(200,30,30,0.35) 0%, rgba(10,10,10,0) 70%)',
      },
      boxShadow: {
        ember: '0 0 40px -5px rgba(249,115,22,0.45)',
        'ember-lg': '0 0 80px -10px rgba(200,30,30,0.55)',
      },
      animation: {
        flicker: 'flicker 3s ease-in-out infinite',
        'fade-up': 'fadeUp 0.6s ease-out both',
        'pop-in': 'popIn 0.25s ease-out both',
        marquee: 'marquee 22s linear infinite',
      },
      keyframes: {
        flicker: {
          '0%, 100%': { opacity: 1 },
          '50%': { opacity: 0.85 },
        },
        fadeUp: {
          '0%': { opacity: 0, transform: 'translateY(24px)' },
          '100%': { opacity: 1, transform: 'translateY(0)' },
        },
        popIn: {
          '0%': { opacity: 0, transform: 'scale(0.92)' },
          '100%': { opacity: 1, transform: 'scale(1)' },
        },
        marquee: {
          '0%': { transform: 'translateX(0)' },
          '100%': { transform: 'translateX(-50%)' },
        },
      },
    },
  },
  plugins: [],
}
