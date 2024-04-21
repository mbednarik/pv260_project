import { Container } from "@mantine/core";
import { QueryErrorResetBoundary } from "@tanstack/react-query";
import { Suspense } from "react";
import { ErrorBoundary } from "react-error-boundary";
import { ErrorFallback } from "~/components/Fallback/ErrorFallback";
import { LoadingFallback } from "~/components/Fallback/LoadingFallback";
import { HoldingDiffsWrapper } from "~/components/HoldingDiffs/HoldingDiffsWrapper";

const App = () => (
	<Container h="100vh">
		<QueryErrorResetBoundary>
			{({ reset }) => (
				<ErrorBoundary onReset={reset} fallbackRender={ErrorFallback}>
					<Suspense fallback={<LoadingFallback />}>
						<HoldingDiffsWrapper />
					</Suspense>
				</ErrorBoundary>
			)}
		</QueryErrorResetBoundary>
	</Container>
);

export default App;
