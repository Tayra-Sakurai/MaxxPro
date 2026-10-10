import type {ReactNode} from 'react';
import clsx from 'clsx';
import Heading from '@theme/Heading';
import Translate, {translate} from '@docusaurus/Translate';
import styles from './styles.module.css';

type FeatureItem = {
  title: ReactNode;
  icon: ReactNode;
  description: ReactNode;
};

const FeatureList: FeatureItem[] = [
  {
    title: (
      <Translate id="feature.localfirst.title">
        完全ローカル & ゼロクラウド
      </Translate>
    ),
    icon: (
      <svg className={styles.featureIcon} viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
        <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
        <path d="m9 12 2 2 4-4" />
      </svg>
    ),
    description: (
      <Translate id="feature.localfirst.description">
        オンラインデータベースや外部クラウドへの依存ゼロ。すべての備蓄データはお手元のPC内（SQLite）に安全に保存され、停電や災害による通信遮断時でも確実に動作します。
      </Translate>
    ),
  },
  {
    title: (
      <Translate id="feature.hierarchy.title">
        3段階分類 & 保管場所管理
      </Translate>
    ),
    icon: (
      <svg className={styles.featureIcon} viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
        <path d="M22 19a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h5l2 3h9a2 2 0 0 1 2 2z" />
        <line x1="12" y1="11" x2="12" y2="17" />
        <line x1="9" y1="14" x2="15" y2="14" />
      </svg>
    ),
    description: (
      <Translate id="feature.hierarchy.description">
        「大分類 ＞ 中分類 ＞ 小分類」の3階層カテゴリと、保管場所（パントリー、非常持出袋、倉庫など）を組み合わせて管理。賞味・消費期限も一目で把握できます。
      </Translate>
    ),
  },
  {
    title: (
      <Translate id="feature.ai.title">
        ローカルAI「Cougar」& 安全承認
      </Translate>
    ),
    icon: (
      <svg className={styles.featureIcon} viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
        <path d="M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z" />
        <circle cx="9" cy="10" r="1" />
        <circle cx="15" cy="10" r="1" />
        <path d="M9.5 13.5c.5.5 1.5.5 2 0" />
      </svg>
    ),
    description: (
      <Translate id="feature.ai.description">
        Ollama（Gemma 4）を連携させた対話型AIアシスタントを内蔵。データの追加・変更・削除時はユーザーによる明示的な「承認」を必須とする安全設計です。
      </Translate>
    ),
  },
  {
    title: (
      <Translate id="feature.fluent.title">
        洗練された Windows 11 ネイティブ
      </Translate>
    ),
    icon: (
      <svg className={styles.featureIcon} viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
        <rect x="2" y="3" width="20" height="14" rx="2" />
        <line x1="8" y1="21" x2="16" y2="21" />
        <line x1="12" y1="17" x2="12" y2="21" />
      </svg>
    ),
    description: (
      <Translate id="feature.fluent.description">
        WinUI 3 と Windows App SDK を採用し、Mica バックドロップと Fluent Design による心地よい操作感を実現。ダークモードや多言語（日本語・英語）に完全対応。
      </Translate>
    ),
  },
];

function Feature({title, icon, description}: FeatureItem) {
  return (
    <div className={clsx('col col--6', styles.featureCol)}>
      <div className={clsx('card', styles.featureCard)}>
        <div className={styles.iconWrapper}>{icon}</div>
        <div className="card__body">
          <Heading as="h3" className={styles.featureTitle}>{title}</Heading>
          <p className={styles.featureDescription}>{description}</p>
        </div>
      </div>
    </div>
  );
}

export default function HomepageFeatures(): ReactNode {
  return (
    <section className={styles.features}>
      <div className="container">
        <div className="row">
          {FeatureList.map((props, idx) => (
            <Feature key={idx} {...props} />
          ))}
        </div>
      </div>
    </section>
  );
}

