<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

# FiAiLLM — FSM_COS / CiAi Integration Contract

**Audience:** the LLM implementing CiAi and its FSM_COS pilot  
**Repository:** `TrentBest/TheSingularityWorkshop.FSM_COS`  
**Related repository:** `TrentBest/TheSingularityWorkshop.CiAi`  
**Purpose:** define the concrete work needed to make CiAi the read-only, evidence-driven CI observer for FSM_COS release readiness. Treat this document as the integration contract and implementation backlog. Where code is missing, implement it; do not downgrade the target architecture merely because the current pilot is incomplete.

## 1. Outcome

CiAi must independently collect, validate, correlate, and report release evidence for an exact FSM_COS commit. It must make evidence traceable to GitHub's authoritative workflow/run/job results and clearly distinguish a verified success from a failure, a skip, a missing check, an in-progress check, and evidence that has not been verified.

The first consumer is FSM_COS release candidate `0.1.0-alpha.7`. The previously examined candidate SHA was `7ea2d9a83d3a4b15408ddb8244389a97e9ad2164`; treat that as a historical pilot fixture, **not** as a permanent pin. Discover and verify the current intended candidate SHA at execution time. Never silently substitute the default branch HEAD for an explicitly requested SHA.

The pilot is **read-only**. It may retrieve GitHub evidence and produce a report/ledger, but must not merge or close pull requests, change branches or tags, dispatch release workflows, publish packages, deploy, or change repository settings.

## 2. Required integration behavior

### 2.1 Target selection and identity

- Accept a target repository in canonical `owner/repository` form and an explicit commit SHA; optionally accept a workflow run ID or pull-request number as a locator.
- For the first pilot, target `TrentBest/TheSingularityWorkshop.FSM_COS`.
- Do not hardcode CiAi as the repository being observed. The current evidence-pilot workflow was found to identify its own repository; generalize it to accept an explicitly configured target.
- Validate all user/workflow inputs. Use least-privilege GitHub permissions and avoid arbitrary shell interpolation.
- Resolve a supplied run ID through GitHub and verify that the returned repository, head SHA, workflow, run ID, and attempt match the requested target. A mismatch is an evidence error, never a pass.
- Record the API retrieval time in UTC and the API URLs/HTML URLs that support each claim. Preserve run attempts so reruns are not accidentally conflated.

### 2.2 Evidence collection

Collect evidence for the exact candidate SHA, including:

1. Build and test workflow, including its jobs and conclusions.
2. Public-package compatibility checks.
3. Source-integration checks.
4. CodeQL/security analysis for the same candidate, where configured.
5. Package creation and package-inspection/compatibility artifacts, where produced.
6. The release workflow's publication job/gate state.
7. Pull-request context and branch/commit identity when applicable.

Discover actual workflow names, job names, run IDs, and artifact names from GitHub. Do not rely only on display-name guesses or a single “latest run” endpoint. Pagination and reruns must be handled. The pilot's known reference run was [FSM_COS Actions run 40060005156](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/40060005156), previously associated with the SHA above; it reported build-and-test, public-package-compatibility, and source-integration successful, publication skipped, and a separate successful CodeQL result. Re-fetch and revalidate all of this from GitHub before reporting it as current evidence.

### 2.3 Evidence model and status vocabulary

Implement or extend CiAi's evidence model so every evidence record carries, at minimum:

- Schema/version identifier.
- Target repository, exact commit SHA, and PR number if relevant.
- Workflow run ID, run attempt, workflow identity, job identity, and conclusion/status where applicable.
- Evidence kind (build/test, compatibility, integration, security, artifact, publication gate, or other explicitly named category).
- Result state, source URL(s), retrieval timestamp, and a concise explanation.
- Failure details: failed job, step, message/annotation, and link when available.
- Artifact name, download/source URL, and digest when available; distinguish the digest of a GitHub artifact archive from the digest of a package inside that archive.
- Whether the record was directly verified, inferred, missing, or stale.

Use a normalized, versioned model rather than treating workflow display strings as the entire schema. Preserve raw provider values alongside normalized values for auditability.

Required normalized states: `passed`, `failed`, `pending`, `skipped`, `missing`, `unknown`, and `unverified`. Keep the original GitHub status/conclusion as a separate field. Never map missing, unknown, skipped, cancelled, neutral, or stale evidence to passed. A skipped publication job can be the expected safe outcome and must still be reported as skipped—not as a successful publication.

### 2.4 Verification and reconciliation

- Reuse and extend `CiRunEvidenceVerifier` rather than duplicating its identity checks.
- Verify repository identity, exact SHA, run ID, attempt, completed status, conclusion, and relevant workflow/job identity before accepting evidence.
- Do not accept an otherwise green run whose head SHA differs from the requested candidate.
- Distinguish “workflow succeeded” from “release requirement satisfied.” A workflow can pass while a required category of evidence is absent.
- Correlate independent runs by exact repository and SHA. Do not combine evidence from different commits into one green report.
- Handle pagination, API errors, rate limits, absent workflows, incomplete runs, reruns, duplicate evidence, and transient failures explicitly.
- For CodeQL, report only what the verified result supports. Do not claim zero findings unless the corresponding candidate's alert/SARIF evidence was actually inspected.
- Artifact upload success does not by itself prove the contents of the package. Where package inspection is required, retrieve the artifact through a supported and authenticated path, verify the artifact and package separately, and record which bytes/digests were checked.

### 2.5 Report and ledger

Produce a human-readable report and machine-readable versioned evidence output. The human report should include:

- Target repository and full commit SHA.
- Collection timestamp.
- One row/section per release requirement with state, evidence URL, and concise rationale.
- Workflow run/job outcomes and relevant rerun/attempt identity.
- Artifact names and digests, with the digest scope clearly identified.
- Missing/unverified items and why they matter.
- Publication gate state, explicitly showing that publication was skipped/disabled unless separately proven otherwise.
- A final overall assessment that does not conceal missing requirements behind an average or percentage.

Persist a reviewable release-evidence ledger in the FSM_COS repository (suggested path: `.github/notes/CIAI_RELEASE_EVIDENCE.md`) or attach the generated report to the pilot run. Do not commit generated evidence automatically from a workflow unless the owner has explicitly approved that behavior. Prefer an uploaded report artifact or a proposed patch for the first pilot.

Readiness percentages may be shown as estimates, but must name the rubric and denominator. Never raise readiness merely because a workflow is green; report outstanding release and branch-reconciliation work separately.

## 3. FSM_COS release-gate requirements

The pilot should report each requirement independently:

- Exact candidate identity and reproducibility.
- Build and test results.
- Public-package compatibility.
- Source-integration compatibility.
- CodeQL/security evidence.
- Package creation and package content/metadata verification.
- Documentation/release notes/version consistency.
- Branch reconciliation and preservation of valuable `master`-only work.
- Publication workflow gate remains disabled/skipped.
- Explicit human release authorization remains outstanding until recorded.

The known FSM_COS PR is [PR #35 — Release gate: FSM_COS 0.1.0-alpha.7](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/35). It is a draft branch-reconciliation/release-gate record, not permission to merge or publish. The branch histories diverge substantially; do not attempt a merge, force-push, reset, branch replacement, or deletion as part of this CiAi pilot.

## 4. Workflow and security constraints

- Use a manually triggered, read-only pilot first. A scheduled or pull-request-triggered observer can be a later increment.
- Request only read permissions needed to inspect actions, checks, contents/PR metadata, security results, and artifacts. Do not request write permissions unless a separately reviewed requirement demonstrates the need.
- Do not expose tokens, secrets, environment contents, private artifact URLs, or sensitive API responses in logs.
- Avoid untrusted-input injection: validate repository names, SHAs, IDs, and URLs; use structured API inputs rather than constructing shell commands.
- Apply timeouts, bounded pagination, retries for transient errors, and clear handling for rate limits.
- Make reruns deterministic and idempotent for a given target SHA and run attempt.
- Do not fetch or execute code from the target repository. Evidence observation must not run the candidate's scripts merely to inspect status.
- Keep provider access behind an abstraction where practical, so future providers do not leak GitHub-specific types into the core evidence model.
- Do not add a runtime dependency to FSM_COS merely to let CiAi inspect it. The observer/integration belongs in CiAi and/or a separate integration package/workflow.

## 5. Implementation plan and acceptance criteria

### Phase A — reusable read-only observer

1. Inspect current CiAi source, tests, contracts, and the `CiRunEvidenceVerifier` implementation.
2. Generalize the existing evidence-pilot workflow from self-repository assumptions to an explicit target repository and SHA/run identity.
3. Implement a GitHub Actions evidence provider/adapter with pagination and stable error handling.
4. Add typed normalized evidence records and an aggregator that requires exact-SHA agreement.
5. Add tests for each state and all mismatch/error cases below.
6. Document the least-privilege permissions and how to run the pilot safely.

**Acceptance:** a manually triggered pilot can observe an arbitrary allowed repository/commit without changing that repository, and produces an auditable report that explicitly separates passed, failed, skipped, missing, pending, and unverified evidence.

### Phase B — FSM_COS alpha.7 pilot

1. Run the observer against the explicitly selected FSM_COS candidate SHA.
2. Confirm every run's repository and head SHA match the requested target.
3. Reconcile build/test, public compatibility, source integration, CodeQL, artifacts, and publication-gate evidence.
4. Produce the report and ledger without modifying branch refs, pull-request state, or package feeds.
5. Compare the report against the actual GitHub Actions pages and correct any false positive/negative.

**Acceptance:** the report can be independently checked from its linked GitHub evidence, contains no cross-SHA aggregation, explains all missing evidence, and does not claim publication occurred when the publish job was skipped.

### Phase C — back-develop the durable integration

After the pilot passes, extract reusable contracts and tests, update CiAi's integration roadmap and release-readiness documentation, and define how FSM_COS and other repositories opt in. Keep remote dispatch/result transport and GUI integration explicitly marked as unimplemented until they pass an end-to-end test.

**Acceptance:** contract tests cover the provider-neutral evidence model; the GitHub adapter is replaceable; an end-to-end test demonstrates request → remote observation → validated result → report; documentation accurately distinguishes implemented behavior from planned behavior.

## 6. Minimum test matrix

Add automated tests for at least:

- Exact repository + SHA + run ID + attempt match → eligible evidence.
- Wrong repository, wrong SHA, wrong run ID, or mismatched attempt → rejected/unverified.
- Completed successful run → passed only for the evidence categories it actually proves.
- Failed job with step/message/URL → failed and diagnostic retained.
- In-progress run → pending.
- Skipped publish job → skipped, never published.
- Missing required workflow/result → missing, not passed.
- Unknown conclusion, cancelled run, neutral result, malformed payload → unknown/unverified, never passed.
- Successful run on a different SHA → rejected for the target candidate.
- Pagination where the matching run is not on the first page.
- Workflow rerun with a new attempt → distinct evidence records; no accidental overwrite.
- API timeout, rate limit, authorization failure, and transient server error → bounded retry/clear error.
- Duplicate records and conflicting results → deterministic reconciliation with conflict visible.
- Artifact archive digest versus package digest → correctly scoped fields and no conflation.
- No write-side effects: no branch update, PR mutation, package publish, deployment, or repository setting change.

## 7. Explicit non-goals for this milestone

Do not block the read-only pilot on a complete CiAi GUI, live dispatch bus, or every future ecosystem integration. Do not claim those features exist. Use local/in-process orchestration where needed for the first vertical proof, but keep contracts suitable for later remote transport.

Do not make CiAi the authority that grants release permission. It supplies validated evidence to the human/operator and existing release gate. A future automated policy gate can be proposed separately, but it must not silently gain publication credentials or release authority.

## 8. Definition of done

- [ ] CiAi can target FSM_COS explicitly instead of observing only itself.
- [ ] All evidence is pinned to the exact repository and candidate SHA.
- [ ] Run attempts, job details, source URLs, and timestamps are retained.
- [ ] Required evidence states and mismatch handling are covered by tests.
- [ ] FSM_COS alpha.7 pilot report reconciles the current GitHub evidence.
- [ ] Report shows publication as skipped/disabled unless independently verified otherwise.
- [ ] The pilot makes no writes to FSM_COS refs, PRs, releases, package feeds, or deployments.
- [ ] CiAi roadmap/readiness docs are updated to separate delivered pilot capability from future work.
- [ ] Known limitations and unimplemented remote/GUI paths remain explicit.

**Operating principle:** observe broadly, verify strictly, report honestly, and do not mutate release state.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
