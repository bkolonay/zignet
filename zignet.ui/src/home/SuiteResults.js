import React, { Component } from 'react';
import SuiteResultChart from './SuiteResultChart'
import GroupSuitesButton from './GroupSuitesButton'

class SuiteResults extends Component {
  render() {
    return (
  	  <div className="container">
        <div className="text-right">
          <GroupSuitesButton grouped={this.props.grouped} />
        </div>
  	  	<div className="row">
          {this.props.suiteResults.map((suiteResult) =>
            <SuiteResultChart key={suiteResult.SuiteIds[0]}
                              suiteResult={suiteResult}
                              grouped={this.props.grouped} />
          )}
        </div>
      </div>
    );
  }
}

export default SuiteResults;
