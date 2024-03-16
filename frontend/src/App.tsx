import { QueryErrorResetBoundary } from "@tanstack/react-query";
import { Suspense } from "react";
import { ErrorBoundary } from "react-error-boundary";
import { Container } from "@mantine/core";
import { ErrorFallback } from "~/components/Fallback/ErrorFallback";
import { HoldingDiffs } from "~/components/HoldingDiffs/HoldingDiffs";
import { LoadingFallback } from "~/components/Fallback/LoadingFallback";

const App = () => (
	<Container h="100vh">
		<QueryErrorResetBoundary>
			{({ reset }) => (
				<ErrorBoundary onReset={reset} fallbackRender={ErrorFallback}>
					<Suspense fallback={<LoadingFallback />}>
						<HoldingDiffs />
					</Suspense>
				</ErrorBoundary>
			)}
		</QueryErrorResetBoundary>
	</Container>
);

export default App;
