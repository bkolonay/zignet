import React, { Component } from 'react';
import TestStatusBadge from './TestStatusBadge'
import TestStatusLabel from './TestStatusLabel'
import TestHistory from './TestHistory'

class SuiteResults extends Component {

  render() {
    const grouped = this.props.grouped;
    const testResult = this.props.testResult;
    return (
  	  <tr>
        <td style={{"width": "52%"}}>{testResult.TestName}</td>
        {grouped && <td><small className="text-muted">{testResult.SuiteName}</small></td>}
        <td style={{"width": "18%"}}>
          <TestStatusBadge failingFromDate={this.props.failingFromDate}/>
          <TestStatusLabel failingFromDate={this.props.failingFromDate} passingFromDate={this.props.passingFromDate}/>
        </td>
        <td style={{"width": "30%"}}><TestHistory testFailureDurations={testResult.TestFailureDurations}/></td>
      </tr>
    );
  }
}

export default SuiteResults;
