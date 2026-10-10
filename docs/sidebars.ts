import type {SidebarsConfig} from '@docusaurus/plugin-content-docs';


const sidebars: SidebarsConfig = {
  docsSidebar: [
    'intro',
    {
      type: 'category',
      label: 'クイックスタート',
      collapsed: false,
      items: [
        'getting-started/quickstart',
      ],
    },
    {
      type: 'category',
      label: 'ユーザーガイド',
      collapsed: false,
      items: [
        'user-guide/items',
        'user-guide/categories',
        'user-guide/places',
        'user-guide/ai-assistant',
      ],
    },
    {
      type: 'category',
      label: 'アーキテクチャ',
      collapsed: false,
      items: [
        'architecture/overview',
        'architecture/data-model',
        'architecture/local-ai',
      ],
    },
    {
      type: 'category',
      label: '開発者ガイド',
      collapsed: false,
      items: [
        'development/building',
        'development/testing',
        'development/contributing',
      ],
    },
  ],
};

export default sidebars;

