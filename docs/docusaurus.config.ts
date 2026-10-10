import {themes as prismThemes} from 'prism-react-renderer';
import type {Config} from '@docusaurus/types';
import type * as Preset from '@docusaurus/preset-classic';

// This runs in Node.js - Don't use client-side code here (browser APIs, JSX...)

const config: Config = {
  title: 'MaxxPro',
  tagline: '完全ローカルで安心の備蓄管理デスクトップアプリ',
  favicon: 'img/logo.png',

  // Future flags, see https://docusaurus.io/docs/api/docusaurus-config#future
  future: {
    v4: true, // Improve compatibility with the upcoming Docusaurus v4
  },

  // Set the production url of your site here
  url: 'https://tayra-sakurai.github.io',
  // Set the /<baseUrl>/ pathname under which your site is served
  // For GitHub pages deployment, it is often '/<projectName>/'
  baseUrl: '/MaxxPro/',

  // GitHub pages deployment config.
  // If you aren't using GitHub pages, you don't need these.
  organizationName: 'Tayra-Sakurai', // Usually your GitHub org/user name.
  projectName: 'MaxxPro', // Usually your repo name.

  onBrokenLinks: 'throw',

  // Even if you don't use internationalization, you can use this field to set
  // useful metadata like html lang. For example, if your site is Chinese, you
  // may want to replace "en" with "zh-Hans".
  i18n: {
    defaultLocale: 'ja-JP',
    locales: ['ja-JP', 'en'],
    localeConfigs: {
      'ja-JP': {
        label: '日本語',
        direction: 'ltr',
        htmlLang: 'ja-JP',
      },
      en: {
        label: 'English',
        direction: 'ltr',
        htmlLang: 'en-US',
      },
    },
  },

  presets: [
    [
      'classic',
      {
        docs: {
          sidebarPath: './sidebars.ts',
        },
        blog: {
          showReadingTime: true,
          feedOptions: {
            type: ['rss', 'atom'],
          },
          onInlineTags: 'warn',
          onInlineAuthors: 'warn',
          onUntruncatedBlogPosts: 'warn',
        },
        theme: {
          customCss: './src/css/custom.css',
        },
      } satisfies Preset.Options,
    ],
  ],

  themeConfig: {
    // Replace with your project's social card
    image: 'img/logo.png',
    colorMode: {
      respectPrefersColorScheme: true,
    },
    navbar: {
      title: 'MaxxPro',
      logo: {
        alt: 'MaxxPro Logo',
        src: 'img/logo.png',
      },
      items: [
        {
          type: 'docSidebar',
          sidebarId: 'docsSidebar',
          position: 'left',
          label: 'ドキュメント',
        },
        { to: '/blog', label: 'ブログ', position: 'left' },
        {
          type: 'localeDropdown',
          position: 'right',
        },
        {
          href: 'https://github.com/Tayra-Sakurai/MaxxPro',
          label: 'GitHub',
          position: 'right',
        },
      ],
    },
    footer: {
      style: 'dark',
      links: [
        {
          title: 'ドキュメント',
          items: [
            {
              label: 'はじめに',
              to: '/docs/intro',
            },
            {
              label: 'クイックスタート',
              to: '/docs/getting-started/quickstart',
            },
            {
              label: 'アーキテクチャ',
              to: '/docs/architecture/overview',
            },
          ],
        },
        {
          title: 'コミュニティ & リンク',
          items: [
            {
              label: 'ブログ',
              to: '/blog',
            },
            {
              label: 'GitHub リポジトリ',
              href: 'https://github.com/Tayra-Sakurai/MaxxPro',
            },
            {
              label: 'リリースページ',
              href: 'https://github.com/Tayra-Sakurai/MaxxPro/releases',
            },
          ],
        },
        {
          title: 'ライセンス',
          items: [
            {
              label: 'GPL-3.0-or-later',
              href: 'https://spdx.org/licenses/GPL-3.0-or-later.html',
            },
          ],
        },
      ],
      copyright: `Copyright © ${new Date().getFullYear()} Tayra Sakurai. Built with Docusaurus. Released under GPL-3.0-or-later.`,
    },
    prism: {
      theme: prismThemes.github,
      darkTheme: prismThemes.dracula,
      additionalLanguages: ['csharp', 'powershell', 'bash', 'json'],
    },
  } satisfies Preset.ThemeConfig,
};

export default config;

