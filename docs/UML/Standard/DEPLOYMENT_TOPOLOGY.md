<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Deployment and Runtime Topology

## Applicability
Use this view when deployment/runtime nodes materially affect behavior. A pure library with no owned deployment topology may mark this view **not applicable**, with a reason.

## Content
Where relevant, show executable processes, hosts, containers, services, storage, network connections, configuration/secret ownership, trust boundaries, failure domains, and deployment artifacts. Label protocols and transport only when verified. Separate build/package-time relationships from runtime communication. Distinguish local test fixtures from production infrastructure.

Do not claim a cloud resource, remote artifact download, sandbox, process boundary, or isolation property unless it exists and is verified. Security boundaries are especially important: an in-process extension is not a sandbox merely because it is called a bundle.

## Acceptance
Every node/edge is labeled as implemented, observed, planned, or conceptual and links to configuration/source/evidence where available.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
