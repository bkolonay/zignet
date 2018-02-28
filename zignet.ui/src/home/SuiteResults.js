import React, { Component } from 'react';

import SuiteResultChart from './SuiteResultChart'

class SuiteResults extends Component {
  constructor(props) {
    super(props);
  }

  render() {
  	const suiteResults = this.props.suiteResults;
    return (
	  <div>
        {suiteResults.map((suiteResult) =>
          <SuiteResultChart key={suiteResult.suiteResultId}
                            suiteName={suiteResult.suiteName} />
        )}
      </div>
    );
  }
}

export default SuiteResults;
