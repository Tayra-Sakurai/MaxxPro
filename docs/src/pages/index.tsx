import type {ReactNode} from 'react';
import clsx from 'clsx';
import Link from '@docusaurus/Link';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import Layout from '@theme/Layout';
import HomepageFeatures from '@site/src/components/HomepageFeatures';
import Heading from '@theme/Heading';
import Translate from '@docusaurus/Translate';

import styles from './index.module.css';

function HomepageHeader() {
  const {siteConfig} = useDocusaurusContext();
  return (
    <header className={clsx('hero hero--primary', styles.heroBanner)}>
      <div className="container">
        <div className={styles.heroLogoWrapper}>
          <img src="img/logo.png" alt="MaxxPro Logo" className={styles.heroLogo} />
        </div>
        <Heading as="h1" className="hero__title">
          {siteConfig.title}
        </Heading>
        <p className="hero__subtitle">
          <Translate id="homepage.tagline">
            完全ローカルで安心の備蓄管理デスクトップアプリ
          </Translate>
        </p>
        <div className={styles.badges}>
          <span className={styles.badge}>Windows 11 / WinUI 3</span>
          <span className={styles.badge}>100% Local-First</span>
          <span className={styles.badge}>Local AI (Ollama)</span>
          <span className={styles.badge}>GPL-3.0</span>
        </div>
        <div className={styles.buttons}>
          <Link
            className="button button--secondary button--lg"
            to="/docs/intro">
            <Translate id="homepage.button.getStarted">
              ドキュメントを見る 📖
            </Translate>
          </Link>
          <Link
            className={clsx('button button--outline button--secondary button--lg', styles.buttonSecondary)}
            href="https://github.com/Tayra-Sakurai/MaxxPro/releases">
            <Translate id="homepage.button.download">
              ダウンロード (Releases) ⬇️
            </Translate>
          </Link>
        </div>
      </div>
    </header>
  );
}

export default function Home(): ReactNode {
  return (
    <Layout
      title="MaxxPro - Local-First Stockpile Management"
      description="MaxxPro is a Windows 11 desktop application for stockpile management without any online database, powered by local AI.">
      <HomepageHeader />
      <main>
        <HomepageFeatures />
      </main>
    </Layout>
  );
}

