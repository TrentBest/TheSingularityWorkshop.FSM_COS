# Preserved remaining SVG artwork from alpha.7 integration branch

Verbatim SVG snapshots from `work/alpha7-living-docs-integration`, reviewed 2026-10-21. Kept as text for recovery and diff review, not promoted over current visuals without regeneration and contract checks.


---

## docs/assets/arbitration-convergence.svg
Source blob: `0b85cb8307ae80e561506dc40ffaae803f41e5b4`

```svg
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1400 700" role="img" aria-labelledby="asset-title asset-desc">
<title id="asset-title">FSM_COS arbitration and convergence</title>
<desc id="asset-desc">Diagram illustrating repeated arbitration rounds and the convergence condition, bounded by a configured maximum round count.</desc>
<defs><linearGradient id="bg" x1="0" x2="1"><stop stop-color="#070b12"/><stop offset="1" stop-color="#111827"/></linearGradient></defs>
<rect width="1400" height="700" rx="26" fill="url(#bg)"/>
<g font-family="Arial,Helvetica,sans-serif">
<text x="70" y="70" fill="#fff" font-size="34" font-weight="700" letter-spacing="3">ARBITRATION → CONVERGENCE</text>
<text x="72" y="101" fill="#76cfff" font-size="15" letter-spacing="2">RECONCILE THE INSTALLED COMPOSITION, THEN HAND OFF A STABLE ASSEMBLY</text>
<g stroke="#31465b" stroke-width="2" fill="#0e1622">
<rect x="80" y="180" width="240" height="150" rx="20"/><rect x="430" y="180" width="240" height="150" rx="20"/>
<rect x="780" y="180" width="240" height="150" rx="20"/><rect x="1130" y="180" width="190" height="150" rx="20"/>
</g>
<g stroke="#76cfff" stroke-width="3"><path d="M320 255h110"/><path d="M670 255h110"/><path d="M1020 255h110"/></g>
<g fill="#fff" text-anchor="middle">
<text x="200" y="225" font-size="22" font-weight="700">Loaded Set</text><text x="200" y="258" font-size="14" fill="#9eb1c4">all reachable bundles</text><text x="200" y="283" font-size="14" fill="#9eb1c4">installed + configured</text>
<text x="550" y="225" font-size="22" font-weight="700">Round 0…N−1</text><text x="550" y="258" font-size="14" fill="#9eb1c4">each bundle arbitrates</text><text x="550" y="283" font-size="14" fill="#ffb84d">changes → repeat</text>
<text x="900" y="225" font-size="22" font-weight="700">Round 1…N−1</text><text x="900" y="258" font-size="14" fill="#9eb1c4">repeat until stable</text><text x="900" y="283" font-size="14" fill="#ffb84d">bounded by max rounds</text>
<text x="1225" y="225" font-size="22" font-weight="700">Stable</text><text x="1225" y="258" font-size="14" fill="#76cfff">no changes</text><text x="1225" y="283" font-size="14" fill="#76cfff">RuntimeAssembly</text>
</g>
<path d="M1010 390C890 390 850 500 700 500C560 500 520 430 520 330" fill="none" stroke="#ffb84d" stroke-width="3" stroke-dasharray="9 9"/>
<text x="70" y="405" fill="#8fa7bc" font-size="17">A bundle may reveal composition changes to other bundles.</text>
<text x="70" y="438" fill="#8fa7bc" font-size="17">A no-change round exits immediately—even the first round (index 0) may converge.</text>
<g stroke="#26384b" fill="#0e1622">
<rect x="80" y="500" width="390" height="90" rx="16"/><rect x="505" y="500" width="390" height="90" rx="16"/><rect x="930" y="500" width="390" height="90" rx="16"/>
</g>
<g text-anchor="middle" fill="#fff"><text x="275" y="535" font-size="18" font-weight="700">NO CHANGE</text><text x="275" y="562" fill="#76cfff" font-size="14">converged</text><text x="700" y="535" font-size="18" font-weight="700">CHANGE</text><text x="700" y="562" fill="#ffb84d" font-size="14">another round</text><text x="1125" y="535" font-size="18" font-weight="700">MAX REACHED</text><text x="1125" y="562" fill="#ff6f6f" font-size="14">fail — no unstable assembly</text></g>
<text x="70" y="645" fill="#6e8499" font-size="14" letter-spacing="2">THE GOAL IS A STABLE COMPOSITION, NOT A UNIVERSAL WINNER</text>
</g></svg>
```


---

## docs/assets/fsm-cos-overview.svg
Source blob: `469e7fe6426e126fd523fa5b35b9327293fd2c3c`

```svg
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1400 760" role="img" aria-labelledby="asset-title asset-desc">
<title id="asset-title">FSM_COS architecture overview</title>
<desc id="asset-desc">Overview of FSM_COS as a composition layer between MicroBundleDomain and consuming applications, with external responsibilities kept outside the kernel.</desc>
<defs>
<linearGradient id="bg" x1="0" x2="1"><stop stop-color="#070b12"/><stop offset="1" stop-color="#101827"/></linearGradient>
<linearGradient id="cos" x1="0" x2="1"><stop stop-color="#0b2430"/><stop offset="1" stop-color="#142d45"/></linearGradient>
<filter id="glow"><feGaussianBlur stdDeviation="5" result="b"/><feMerge><feMergeNode in="b"/><feMergeNode in="SourceGraphic"/></feMerge></filter>
</defs>
<rect width="1400" height="760" rx="28" fill="url(#bg)"/>
<g font-family="Arial,Helvetica,sans-serif" fill="#e9f5ff">
<text x="70" y="82" font-size="38" font-weight="700" letter-spacing="4">FSM_COS</text>
<text x="72" y="118" font-size="17" fill="#76cfff" letter-spacing="3">COMPOSITION KERNEL / RUNTIME ASSEMBLY</text>
<text x="70" y="178" font-size="16" fill="#9eb1c4">The manifest requests. FSM_COS composes. The host decides what happens next.</text>
</g>
<g stroke="#31465b" stroke-width="2" fill="none">
<path d="M300 320H430"/><path d="M700 320H830"/>
<path d="M1050 320H1180"/><path d="M1265 390V520"/>
</g>
<g fill="#76cfff" filter="url(#glow)">
<path d="M416 311l18 9-18 9z"/><path d="M816 311l18 9-18 9z"/><path d="M1166 311l18 9-18 9z"/><path d="M1256 502l9 18 9-18z"/>
</g>
<g stroke="#26384b" stroke-width="2">
<rect x="70" y="260" width="230" height="120" rx="18" fill="#0e1622"/>
<rect x="430" y="220" width="270" height="200" rx="22" fill="url(#cos)" stroke="#4fb8e8"/>
<rect x="830" y="250" width="220" height="140" rx="18" fill="#0e1622"/>
<rect x="1180" y="250" width="170" height="140" rx="18" fill="#111a25"/>
<rect x="1180" y="520" width="170" height="110" rx="18" fill="#111a25"/>
</g>
<g font-family="Arial,Helvetica,sans-serif" text-anchor="middle">
<text x="185" y="305" fill="#fff" font-size="23" font-weight="700">RuntimeManifest</text>
<text x="185" y="337" fill="#8fa7bc" font-size="15">published runtime request</text>
<text x="565" y="278" fill="#fff" font-size="30" font-weight="700">FSM_COS</text>
<text x="565" y="312" fill="#76cfff" font-size="15">resolve</text><text x="565" y="338" fill="#76cfff" font-size="15">load</text><text x="565" y="364" fill="#ffb84d" font-size="15">arbitrate</text><text x="565" y="390" fill="#8fa7bc" font-size="14">assemble</text>
<text x="940" y="305" fill="#fff" font-size="22" font-weight="700">RuntimeAssembly</text>
<text x="940" y="334" fill="#8fa7bc" font-size="14">stable composition</text>
<text x="1265" y="305" fill="#fff" font-size="21" font-weight="700">Host</text>
<text x="1265" y="335" fill="#8fa7bc" font-size="14">execution</text>
<text x="1265" y="360" fill="#8fa7bc" font-size="14">manifestation</text>
<text x="1265" y="566" fill="#fff" font-size="20" font-weight="700">Experience</text>
<text x="1265" y="596" fill="#8fa7bc" font-size="13">what is encountered</text>
</g>
<text x="70" y="690" fill="#6e8499" font-family="Arial,Helvetica,sans-serif" font-size="15">COMPOSITION IS NOT MANIFESTATION</text>
<text x="1330" y="690" text-anchor="end" fill="#6e8499" font-family="Arial,Helvetica,sans-serif" font-size="15">THE SINGULARITY WORKSHOP</text>
</svg>
```
